using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public LevelScenePm(Ctx ctx)
    {
        _ctx = ctx;

        LoadPrefabs();
        CreateObjects();

        _ctx.onInteractionButtonClick.Subscribe(OnInteractionButtonClick);
    }

    private void LoadPrefabs()
    {
    }

    private void CreateObjects()
    {
        _interactables = new List<InteractableEntity>();
        var table = UnityEngine.GameObject.Instantiate<GameObject>(_ctx.gameSet.table);

        var onInteractStates = new ReactiveCommand<(InteractStates, PointerEventData)>();
        var onSelect = new ReactiveCommand<InteractableView>();
        onSelect.Subscribe(OnInteractableSelected);
        foreach (var set in _ctx.gameSet.interactableSets)
        {
            for (var i = 0; i < set.amount; i++)
            {
                var view = UnityEngine.GameObject.Instantiate<InteractableView>(set.prefab);
                
                InteractableEntity interactableEntity = new InteractableEntity(new InteractableEntity.Ctx
                {
                    operationsSet = _ctx.operationsSet,
                    view = view,
                    type = set.type,
                    operations = set.operations,
                });

                view.SetCtx(new InteractableView.Ctx
                {
                    onSelect = onSelect,
                    operations = set.operations,
                    
                });

                _interactables.Add(interactableEntity);
            }
        }
    }

    private void OnInteractableSelected(InteractableView view)
    {
        var interactable = _interactables.FirstOrDefault(i => i.Data.view.Equals(view));

        if (interactable != null)
        {
            if (_current != interactable)
            {
                _current = interactable;
                _ctx.onSelectInteractable.Execute(interactable.Data.operations);
                // TODO select (visual)
            }
            else
            {
                if (_current != null)
                {
                    // TODO unselect
                    // TODO unselect visual
                }
            }
        }
        else
        {
            Debug.LogError($"[LevelScenePm] Interactable with id {view.name} does not exist");
        }
    }

    private void OnInteractionButtonClick(OperationTypes operation)
    {
        Debug.Log($"[LevelScenePm] OnInteractionButtonClick");
        if (_current == null)
            return;
        
        _current.DoOperation (operation); 
        {
            
        }
        
    }

    public void Dispose()
    {
    }
}