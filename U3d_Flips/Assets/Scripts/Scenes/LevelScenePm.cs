using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class LevelScenePm : IDisposable
{
    public struct Ctx
    {
        public GameSet gameSet;
        public OperationsSet operationsSet;
        public ReactiveCommand<List<OperationTypes>> onSelectInteractable;
        public ReactiveCommand<OperationTypes> onInteractionButtonClick;
    }

    private Ctx _ctx;
    private List<InteractableEntity> _interactables;
    private InteractableEntity _current;
    private ReactiveCommand<OperationTypes> _onDoOperation;

    public LevelScenePm(Ctx ctx)
    {
        _ctx = ctx;
        _interactables = new List<InteractableEntity>();
        CreateObjects();

        _ctx.onInteractionButtonClick.Subscribe(OnInteractionButtonClick);
    }

    private void CreateObjects()
    {
        var table = UnityEngine.Object.Instantiate(_ctx.gameSet.table);

        var onSelect = new ReactiveCommand<InteractableEntity>();
        onSelect.Subscribe(OnInteractableSelected);
        foreach (var set in _ctx.gameSet.interactableSets)
        {
            for (var i = 0; i < set.amount; i++)
            {
                var interactableEntity = new InteractableEntity(new InteractableEntity.Ctx
                {
                    prefab = set.prefab,
                    operationsSet = _ctx.operationsSet,
                    type = set.type,
                    operations = set.operations,
                    onSelect = onSelect,
                });

                _interactables.Add(interactableEntity);
            }
        }
    }

    private void OnInteractableSelected(InteractableEntity interactable)
    {
        if (_current != null)
        {
            // TODO unselect visual
        }

        if (_current == interactable)
        {
            _current = null;
            _ctx.onSelectInteractable.Execute(new List<OperationTypes>());
        }
        else
        {
            _current = interactable;
            _ctx.onSelectInteractable.Execute(interactable.Data.operations);
            // TODO select (visual)
        }
    }

    private void OnInteractionButtonClick(OperationTypes operation)
    {
        Debug.Log($"[LevelScenePm] OnInteractionButtonClick");
        _current?.DoOperation(operation);
    }

    public void Dispose()
    {
    }
}