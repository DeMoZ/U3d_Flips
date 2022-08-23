using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

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
    private ReactiveProperty<Vector3> _mousePosition;
    private ReactiveCommand _onDragObject;

    public LevelScenePm(Ctx ctx)
    {
        _ctx = ctx;
        _disposables = new List<IDisposable>();
        _interactables = new List<InteractableEntity>();
        _current = new ReactiveProperty<InteractableEntity>();
        _mousePosition = new ReactiveProperty<Vector3>();
        _onDragObject = new ReactiveCommand();

        var mouseHandler = new MouseHandler(new MouseHandler.Ctx
        {
            camera = _ctx.camera,
            current = _current,
            interactables = _interactables,
            onDragObject = _onDragObject,
            mousePosition = _mousePosition,
        }).AddTo(_disposables);

        _current.SkipLatestValueOnSubscribe().Subscribe(OnCurrentChange).AddTo(_disposables);
        _onDragObject.Subscribe(_ => OnDragObject()).AddTo(_disposables);
        _ctx.onInteractionButtonClick.Subscribe(OnInteractionButtonClick).AddTo(_disposables);

        CreateObjects();
    }

    private void CreateObjects()
    {
        // TODO here can be used overlap box

        var table = UnityEngine.Object.Instantiate(_ctx.gameSet.table);
        var tBounds = table.GetComponent<Renderer>().bounds;
        var tCenter = tBounds.center;
        var tExtents = tBounds.extents;

        foreach (var set in _ctx.gameSet.interactableSets)
        {
            for (var i = 0; i < set.amount; i++)
            {
                var oBounds = set.prefab.GetComponent<Renderer>().bounds;
                var oExtents = oBounds.extents;

                var x = Random.Range(-tExtents.x + oExtents.x, tExtents.x - oExtents.x);
                var z = Random.Range(-tExtents.z + oExtents.z, tExtents.z - oExtents.z);
                var position = new Vector3(x, tExtents.y + oExtents.y + tExtents.y, z) + tCenter;

                var interactableEntity = new InteractableEntity(new InteractableEntity.Ctx
                {
                    camera = _ctx.camera,
                    prefab = set.prefab,
                    operationsSet = _ctx.operationsSet,
                    type = set.type,
                    operations = set.operations,
                    mousePosition = _mousePosition,
                    position = position,
                    extents = oExtents,
                });

                _interactables.Add(interactableEntity);
            }
        }
    }

    private void OnCurrentChange(InteractableEntity current)
    {
        if (current == null)
        {
            if (_previous != null)
            {
                // TODO _previous.BackToNormal();
                _previous = null;
            }

            _ctx.onSelectInteractable.Execute(new List<OperationTypes>());
        }
        else
        {
            if (_previous != current)
            {
                // TODO _previous.BackToNormal();
                _previous = current;
                // TODO current.SetSelected();
                _ctx.onSelectInteractable.Execute(current.Data.operations);
            }
        }
    }

    private void OnDragObject()
    {
        Debug.Log($"[LevelScenePm] OnMouseDrag, _current = {_current.Value.View.name} :  {_mousePosition}");
        // TODO The drag operation way to different from the rest so it is better to separate from common operations logic
        _current.Value?.DoOperation(OperationTypes.Drag);
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