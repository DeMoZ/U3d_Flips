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

        // Load scene object from scriptable object

        // Load scriptable obj with collection

        // instantiate objects here or on Enter() ?

        // Dependencies
        // Create object with pool. Get object type?
        // object has Entity, view class (View?) with its params where listed its possibilities.
        // when any object deselect, stirked the event to free ui.
        // when the object selected, strike the event to fill the ui with buttons with possible events for selected obj 
    }

    public void Enter()
    {
        var onSelectInteractable = new ReactiveCommand<List<InteractionTypes>>();
        var operationButtonSets = Resources.Load<OperationButtonSets>("OperationButtonSets");
        var gameSet = Resources.Load<GameSet>("GameSet");
        var menuScenePm = new LevelScenePm(new LevelScenePm.Ctx
        {
            tablePrefab = gameSet.table,
            interactables = gameSet.interactableSets,
            onSelectInteractable = onSelectInteractable,
        });

        // Find UI or instantiate from Addressable
        // _ui = Addressable.Instantiate();
        _ui = UnityEngine.GameObject.FindObjectOfType<UiLevelScene>();

        _ui.SetCtx(new UiLevelScene.Ctx
        {
            operationButtonSets = operationButtonSets,
            onSelectInteractable = onSelectInteractable,

        });

        Debug.Log("[LevelSceneEntity] Enter");
    }

    public void Exit()
    {
    }

    public void Dispose()
    {
    }
}

public class Interactable
{
    public struct Ctx
    {
        public InteractableView view;
        public InteractableTypes type;
        public List<InteractionTypes> operations;
    }

    private Ctx _ctx;

    public Ctx Data => _ctx;

    public Interactable(Ctx ctx)
    {
        _ctx = ctx;
    }
}

public interface IInteraction
{
    Task Do();
}

public class Flip : IInteraction
{
    public async Task Do()
    {
    }
}

public class Rotate : IInteraction
{
    public async Task Do()
    {
    }
}