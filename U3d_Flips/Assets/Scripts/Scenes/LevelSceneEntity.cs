using System;
using System.Collections.Generic;
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
    private List<IDisposable> _disposables;

    public LevelSceneEntity(Ctx ctx)
    {
        _ctx = ctx;
        _disposables = new();
    }

    public void Enter()
    {
        var operationsSet = Resources.Load<OperationsSet>("OperationsSet");
        var gameSet = Resources.Load<GameSet>("GameSet");

        var onSelectInteractable = new ReactiveCommand<List<OperationTypes>>();
        var onInteractionButtonClick = new ReactiveCommand<OperationTypes>();

        // from prefab, or find, or addressable
        var camera = UnityEngine.GameObject.FindObjectOfType<Camera>();
        _ui = UnityEngine.GameObject.FindObjectOfType<UiLevelScene>();
        var uiPool = new Pool(new GameObject("uiPool").transform);

        var menuScenePm = new LevelScenePm(new LevelScenePm.Ctx
        {
            camera = camera,
            gameSet = gameSet,
            operationsSet = operationsSet,
            onSelectInteractable = onSelectInteractable,
            onInteractionButtonClick = onInteractionButtonClick,
        }).AddTo(_disposables);


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
        foreach (var d in _disposables)
            d.Dispose();
    }
}