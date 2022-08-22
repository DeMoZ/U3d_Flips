using System.Collections.Generic;
using System.Threading.Tasks;
using UI;
using UniRx;
using UnityEngine;

public class LevelSceneEntity : IGameScene
{
    public struct Ctx
    {
    }

    private Ctx _ctx;
    private UiLevelScene _ui;
    private Dictionary<InteractableTypes, int> _amountData;

    public LevelSceneEntity(Ctx ctx)
    {
        _ctx = ctx;
    }

    public void Enter()
    {
        var operationsSet = Resources.Load<OperationsSet>("OperationsSet");
        var gameSet = Resources.Load<GameSet>("GameSet");

        var onSelectInteractable = new ReactiveCommand<List<OperationTypes>>();
        var onInteractionButtonClick = new ReactiveCommand<OperationTypes>();

        var menuScenePm = new LevelScenePm(new LevelScenePm.Ctx
        {
            gameSet = gameSet,
            operationsSet = operationsSet,
            onSelectInteractable = onSelectInteractable,
            onInteractionButtonClick = onInteractionButtonClick,
        });

        var uiPool = new Pool(new GameObject("uiPool").transform);
        // Find UI or instantiate with code or from Addressable
        // _ui = Addressable.Instantiate();
        _ui = UnityEngine.GameObject.FindObjectOfType<UiLevelScene>();

        _ui.SetCtx(new UiLevelScene.Ctx
        {
            gameSet = gameSet,
            operationsSet = operationsSet,
            onSelectInteractable = onSelectInteractable,
            onInteractionButtonClick = onInteractionButtonClick,
            pool = uiPool,
        });

        Debug.Log("[LevelSceneEntity] Entered");
    }

    public void Exit()
    {
    }

    public void Dispose()
    {
    }
}