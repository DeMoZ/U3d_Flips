using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelScenePm : IDisposable
{
    public struct Ctx
    {
        public GameObject tablePrefab;
        public List<InteractableSet> interactables;
        public ReactiveCommand<List<InteractionTypes>> onSelectInteractable;
    }

    private Ctx _ctx;
    private List<Interactable> _interactables;
    private Interactable _current;
    
    public LevelScenePm(Ctx ctx)
    {
        _ctx = ctx;
        LoadPrefabs();
        CreateObjects();
    }

    private void LoadPrefabs()
    {
        
    }
    
    private void CreateObjects()
    {
        _interactables = new List<Interactable>();
        var table = UnityEngine.GameObject.Instantiate<GameObject>(_ctx.tablePrefab);

        var onDragStates = new ReactiveCommand<(DragStates, PointerEventData)>();
        
        foreach (var set in _ctx.interactables)
        {
            for (var i = 0; i < set.amount; i++)
            {
                var view = UnityEngine.GameObject.Instantiate<InteractableView>(set.prefab);
                
                view.SetCtx(new InteractableView.Ctx
                {
                    onDragStates = onDragStates,
                });

                Interactable interactable = new Interactable(new Interactable.Ctx
                {
                    view = view,
                    type = set.type,
                });
                
                _interactables.Add(interactable);
            }
        }
    }
    
    public void Dispose()
    {
        
    }
}