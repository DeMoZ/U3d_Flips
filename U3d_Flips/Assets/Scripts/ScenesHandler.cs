using System;
using System.Threading.Tasks;
using UniRx;

public class ScenesHandler : IDisposable
{
    public struct Ctx
    {
        public string startApplicationSceneName;
        public ReactiveCommand onStartApplicationSwitchScene;
        public ReactiveCommand<GameScenes> onSwitchScene;
    }

    private const string ROOT_SCENE = "1_RootScene";
    private const string SWITCH_SCENE = "2_SwitchScene";
    private const string MENU_SCENE = "MenuScene";
    private const string LEVEL_SCENE = "LevelScene";

    private Ctx _ctx;
    private CompositeDisposable _disposables;

    public string RootScene => ROOT_SCENE;
    public string MenuScene => MENU_SCENE;
    public string SwitchScene => SWITCH_SCENE;
    public string LevelScene => LEVEL_SCENE;

    public ScenesHandler(Ctx ctx)
    {
        _ctx = ctx;
        _disposables = new CompositeDisposable();
        _ctx.onStartApplicationSwitchScene.Subscribe(_ => SelectSceneForStartApplication()).AddTo(_disposables);
    }


    private void SelectSceneForStartApplication()
    {
        switch (_ctx.startApplicationSceneName)
        {
            // case ROOT_SCENE:
            //     _ctx.onSwitchScene.Execute(GameScenes.Menu);
            //     break;
            // case MENU_SCENE:
            //     _ctx.onSwitchScene.Execute(GameScenes.Menu);
            //     break;
            // case SWITCH_SCENE:
            //     _ctx.onSwitchScene.Execute(GameScenes.Menu);
            //     break;
            case LEVEL_SCENE:
                _ctx.onSwitchScene.Execute(GameScenes.Level1);
                break;
            default:
                _ctx.onSwitchScene.Execute(GameScenes.Menu);
                break;
        }
    }

    public string GetSceneName(GameScenes scene)
    {
        return scene switch
        {
            GameScenes.Menu => MenuScene,
            GameScenes.Level1 => LevelScene,
            _ => throw new ArgumentOutOfRangeException(nameof(scene), scene, null)
        };
    }
    
    public async Task<IGameScene> SceneEntity(GameScenes scene)
    {
        IGameScene newScene = scene switch
        {
            GameScenes.Menu => LoadMenu(),
            GameScenes.Level1 => await LoadLevel1(),
            _ => LoadMenu()
        };

        return newScene;
    }
    
    private IGameScene LoadMenu()
    {
        return new MenuSceneEntity(new MenuSceneEntity.Ctx
        {
            scene = GameScenes.Menu,
            onSwitchScene = _ctx.onSwitchScene,
        });
    }

    private async Task<IGameScene> LoadLevel1()
    {
        var constructorTask = new Container<Task>();
        var sceneEntity = new LevelSceneEntity(new LevelSceneEntity.Ctx
        {
            constructorTask = constructorTask,
            onSwitchScene = _ctx.onSwitchScene,
        });

        await constructorTask.Value;
        return sceneEntity;
    }
    
    public void Dispose()
    {
        _disposables.Dispose();
    }
}