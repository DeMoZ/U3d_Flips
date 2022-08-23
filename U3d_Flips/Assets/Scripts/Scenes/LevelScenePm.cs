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

    public LevelScenePm(Ctx ctx)
    {
        _ctx = ctx;
        _disposables = new List<IDisposable>();
        _interactables = new List<InteractableEntity>();
        _current = new ReactiveProperty<InteractableEntity>();
        _mousePosition = new ReactiveProperty<Vector3>();

        var mouseHandler = new MouseHandler(new MouseHandler.Ctx
        {
            camera = _ctx.camera,
            current = _current,
            interactables = _interactables,
            mousePosition = _mousePosition,
        }).AddTo(_disposables);

        _current.Subscribe(OnCurrentChange).AddTo(_disposables);
        _ctx.onInteractionButtonClick.Subscribe(OnInteractionButtonClick).AddTo(_disposables);

        CreateObjects();
    }

    private void CreateObjects()
    {
        var table = UnityEngine.Object.Instantiate(_ctx.gameSet.table);
        var tBounds = table.GetComponent<Renderer>().bounds;
        var tCenter = tBounds.center;
        var tExtents = tBounds.extents;

        foreach (var set in _ctx.gameSet.interactableSets)
        {
            for (var i = 0; i < set.amount; i++)
            {
                var x = Random.Range(-tExtents.x, tExtents.x);
                var z = Random.Range(-tExtents.z, tExtents.z);
                Vector3 position = new Vector3(x, tExtents.y, z);
                position += tCenter;
                
                var interactableEntity = new InteractableEntity(new InteractableEntity.Ctx
                {
                    prefab = set.prefab,
                    operationsSet = _ctx.operationsSet,
                    type = set.type,
                    operations = set.operations,
                    mousePosition = _mousePosition,
                    position = position,
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
                //_previous.BackToNormal();
                _previous = null;
            }
        }
        else
        {
            if (_previous != current)
            {
                //_previous.BackToNormal();
                _previous = current;
                // current.SetSelected();
                _ctx.onSelectInteractable.Execute(current.Data.operations);
            }
        }
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