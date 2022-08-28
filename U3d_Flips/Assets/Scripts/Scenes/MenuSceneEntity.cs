using UI;
using UniRx;

public class MenuSceneEntity : IGameScene
{
    public struct Ctx
    {
        public GameScenes scene;
        public ReactiveCommand<GameScenes> onSwitchScene;
        public ReactiveCommand onClickPlay;
    }

    private Ctx _ctx;
    private UiMenuScene _ui;

    private ReactiveCommand _onClickNewGame;
    private ReactiveCommand _onClickSettings;

    public MenuSceneEntity(Ctx ctx)
    {
        _ctx = ctx;

        _onClickNewGame = new();
        _onClickSettings = new();
    }

    public void Enter()
    {
        var menuScenePm = new MenuScenePm(new MenuScenePm.Ctx
        {
        });
        
        // Find UI or instantiate from Addressable
        // _ui = Addressable.Instantiate();
        _ui = UnityEngine.GameObject.FindObjectOfType<UiMenuScene>();
        
        _ui.SetCtx(new UiMenuScene.Ctx
        {
            onSwitchScene = _ctx.onSwitchScene,
            onClickNewGame = _onClickNewGame,
            onClickSettings = _onClickSettings,
        });
    }

    public void Exit()
    {
    }

    public void Dispose()
    {
    }
}