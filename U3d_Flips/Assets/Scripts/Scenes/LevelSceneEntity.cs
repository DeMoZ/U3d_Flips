using System.Collections.Generic;
using System.Threading.Tasks;
using Configs;
using DataLoad;
using UI;
using UniRx;
using UnityEngine;

public class LevelSceneEntity : IGameScene
{
    public struct Ctx
    {
        public Container<Task> constructorTask;
        public ReactiveCommand<GameScenes> onSwitchScene;
    }

    private Ctx _ctx;
    private UiLevelScene _ui;
    private Dictionary<InteractableTypes, int> _amountData;
    private List<Texture2D> _textures;
    private CompositeDisposable _disposables;

    public LevelSceneEntity(Ctx ctx)
    {
        _ctx = ctx;
        _disposables = new ();

        AsyncConstructor();
    }

    private void AsyncConstructor()
    {
        _ctx.constructorTask.Value = ConstructorTask();
    }

    private async Task ConstructorTask()
    {
        var imageLoader = new StrAssetImageLoader("Textures");
        _disposables.Add(imageLoader);
        _textures = await imageLoader.LoadImages();
        
        // await Task.Yield();
        // await Task.Delay(5 * 1000);
    }

    public void Enter()
    {
        var operationsSet = Resources.Load<OperationsSet>("OperationsSet");
        var gameSet = Resources.Load<GameSet>("GameSet");

        var onSelectInteractable = new ReactiveCommand<List<OperationTypes>>().AddTo(_disposables);
        var onInteractionButtonClick = new ReactiveCommand<OperationTypes>().AddTo(_disposables);

        // from prefab, or find, or addressable
        _ui = UnityEngine.GameObject.FindObjectOfType<UiLevelScene>();
        var camera = _ui.Camera;
        var uiPool = new Pool(new GameObject("uiPool").transform);

        var scenePm = new LevelScenePm(new LevelScenePm.Ctx
        {
            camera = camera,
            gameSet = gameSet,
            operationsSet = operationsSet,
            onSelectInteractable = onSelectInteractable,
            onInteractionButtonClick = onInteractionButtonClick,
            textures = _textures,
        }).AddTo(_disposables);


        _ui.SetCtx(new UiLevelScene.Ctx
        {
            gameSet = gameSet,
            operationsSet = operationsSet,
            onSelectInteractable = onSelectInteractable,
            onInteractionButtonClick = onInteractionButtonClick,
            pool = uiPool,
            onSwitchScene = _ctx.onSwitchScene,
        });

        Debug.Log("[LevelSceneEntity] Entered");
    }

    public void Exit()
    {
    }

    public void Dispose()
    {
        _disposables.Dispose();
        Resources.UnloadUnusedAssets();
    }
}