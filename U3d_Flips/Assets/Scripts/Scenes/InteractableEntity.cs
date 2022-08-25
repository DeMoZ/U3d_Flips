using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Operations;
using UniRx;
using UnityEngine;

public class InteractableEntity : IDisposable
{
    public struct Ctx
    {
        public Camera camera;
        public InteractableView prefab;
        public OperationsSet operationsSet;
        public InteractableTypes type;
        public List<OperationTypes> operations;
        public ReactiveProperty<Vector3> mousePosition;
        public Vector3 position;
        public Vector3 extents;
        public Texture2D texture;
        public ReactiveCommand<(InteractableEntity entity, bool isSelected)> onColorChange;
    }

    private Ctx _ctx;

    private List<AbstractOperation> _operations;
    private InteractableView _view;
    private ReactiveCommand<OperationTypes> _onDoOperation;
    private CompositeDisposable _disposables;

    public Ctx Data => _ctx;

    public InteractableView View => _view;

    public InteractableEntity(Ctx ctx)
    {
        _ctx = ctx;
        _disposables = new CompositeDisposable();
        
        _onDoOperation = new ReactiveCommand<OperationTypes>().AddTo(_disposables);
        var onMouseStates = new ReactiveCommand<MouseStates>();
        onMouseStates.Subscribe(OnMouseStates).AddTo(_disposables);
        var onColorChange = new ReactiveCommand<bool>();
        _ctx.onColorChange.Subscribe(command =>
        {
            if (command.entity == this)
                onColorChange.Execute(command.isSelected);
        }).AddTo(_disposables);
        
        _view = UnityEngine.Object.Instantiate(_ctx.prefab, _ctx.position, Quaternion.identity);

        _view.SetCtx(new InteractableView.Ctx
        {
            texture = _ctx.texture,
            onColorChange = onColorChange,
        });
        
        _operations = new();
        _operations.AddRange(_view.gameObject.GetComponents<AbstractOperation>());

        foreach (var ctxOperation in _ctx.operations)
        {
            var o = _operations.FirstOrDefault(o => o.GetOperationType == ctxOperation);
            if (o == null)
                o = AddOperation(ctxOperation);
        }
    }

    private void OnMouseStates(MouseStates mouseStates)
    {
        switch (mouseStates)
        {
            case MouseStates.Down:
                break;
            case MouseStates.Up:
                break;
            case MouseStates.Drag:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(mouseStates), mouseStates, null);
        }
    }
    
    private AbstractOperation AddOperation(OperationTypes type)
    {
        var time = _ctx.operationsSet.GetOperation(type).duration;
        switch (type)
        {
            case OperationTypes.Flip:
                var flip = _view.gameObject.AddComponent<Flip>();
                flip.SetCtx(new Flip.Ctx
                {
                    time = time,
                    onDoOperation = _onDoOperation,
                });
                _operations.Add(flip);
                return flip;
            
            case OperationTypes.Rotate:
                var rotate = _view.gameObject.AddComponent<Rotate>();
                rotate.SetCtx(new Rotate.Ctx
                {
                    time = time,
                    onDoOperation = _onDoOperation,
                });
                _operations.Add(rotate);
                return rotate;
            
            case OperationTypes.Drag:
                var drag = _view.gameObject.AddComponent<Drag>();
                drag.SetCtx(new Drag.Ctx
                {
                    camera = _ctx.camera,
                    onDoOperation = _onDoOperation,
                    mousePosition = _ctx.mousePosition,
                    extents = _ctx.extents,
                    time = time,
                });
                _operations.Add(drag);
                return drag;

            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public void DoOperation(OperationTypes operation)
    {
        _onDoOperation.Execute(operation);
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}