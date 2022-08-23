using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class LevelScenePm : IDisposable
{
    public struct Ctx
    {
        public Camera camera;
        public GameSet gameSet;
        public OperationsSet operationsSet;
        public ReactiveCommand<List<OperationTypes>> onSelectInteractable;
        public ReactiveCommand<OperationTypes> onInteractionButtonClick;
    }

    private Ctx _ctx;
    private List<InteractableEntity> _interactables;
    private ReactiveProperty<InteractableEntity> _current;
    private InteractableEntity _previous;
    private ReactiveCommand<OperationTypes> _onDoOperation;
    private List<IDisposable> _disposables;

    public LevelScenePm(Ctx ctx)
    {
        _ctx = ctx;
        _disposables = new();
        _interactables = new List<InteractableEntity>();
        _current = new ReactiveProperty<InteractableEntity>();
        CreateObjects();
        _ctx.onInteractionButtonClick.Subscribe(OnInteractionButtonClick).AddTo(_disposables);
    }

    private void CreateObjects()
    {
        var table = UnityEngine.Object.Instantiate(_ctx.gameSet.table);

        // var onSelect = new ReactiveCommand<InteractableEntity>();
        // onSelect.Subscribe(OnInteractableSelected).AddTo(_disposables);
        
        var mousePosition = new ReactiveProperty<Vector3>();
        
        var mouseHandler = new MouseHandler(new MouseHandler.Ctx
        {
            camera = _ctx.camera,
            current = _current,
            interactables = _interactables,
            mousePosition = mousePosition,
            //onSelectInteractable = 
        }).AddTo(_disposables);


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
                    mousePosition = mousePosition,
                });

                _interactables.Add(interactableEntity);
            }
        }

        _current.Subscribe(current =>
        {
            if (current == null)
            {
                if (_previous != null)
                {
                    //_previous.BackToNormal();
                }
            }
            else
            {
                if (_previous != current)
                {
                    //_previous.BackToNormal();
                    // current.SetSelected();
                     _ctx.onSelectInteractable.Execute(current.Data.operations);
                }
            }
            
        }).AddTo(_disposables);
    }

    

    private void OnInteractionButtonClick(OperationTypes operation)
    {
        Debug.Log($"[LevelScenePm] OnInteractionButtonClick");
        _current.Value?.DoOperation(operation);
    }

    public void Dispose()
    {
        foreach (var d in _disposables)
            d.Dispose();
    }
}