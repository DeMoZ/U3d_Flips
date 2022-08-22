using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public class InteractableEntity
{
    public struct Ctx
    {
        public InteractableView prefab;
        public OperationsSet operationsSet;
        public InteractableTypes type;
        public List<OperationTypes> operations;
        public ReactiveCommand<InteractableEntity> onSelect;
    }

    private Ctx _ctx;

    private List<AbstractOperation> _operations;
    private InteractableView _view;
    private ReactiveCommand<OperationTypes> _onDoOperation;

    public Ctx Data => _ctx;

    public InteractableView View => _view;

    public InteractableEntity(Ctx ctx)
    {
        _ctx = ctx;
        _onDoOperation = new ReactiveCommand<OperationTypes>();
        
        _view = UnityEngine.GameObject.Instantiate(_ctx.prefab);
        var onSelect = new ReactiveCommand();
        onSelect.Subscribe(_ =>
        {
            _ctx.onSelect.Execute(this);
        });
        
        View.SetCtx(new InteractableView.Ctx
        {
            onSelect = onSelect,
            operations = _ctx.operations,
        });
        
        _operations = new();
        _operations.AddRange(View.gameObject.GetComponents<AbstractOperation>());

        foreach (var ctxOperation in _ctx.operations)
        {
            var o = _operations.FirstOrDefault(o => o.GetOperationType == ctxOperation);
            if (o == null)
                o = AddOperation(ctxOperation);
        }
    }

    private AbstractOperation AddOperation(OperationTypes type)
    {
        var time = _ctx.operationsSet.GetOperation(type).duration;
        switch (type)
        {
            case OperationTypes.Flip:
                var flip = View.gameObject.AddComponent<Flip>();
                flip.SetCtx(new AbstractOperation.Ctx
                {
                    time = time,
                    onDoOperation = _onDoOperation,
                });
                _operations.Add(flip);
                return flip;
                break;
            case OperationTypes.Rotate:
                var rotate = View.gameObject.AddComponent<Rotate>();
                rotate.SetCtx(new AbstractOperation.Ctx
                {
                    time = time,
                    onDoOperation = _onDoOperation,
                });
                _operations.Add(rotate);
                return rotate;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public void DoOperation(OperationTypes operation)
    {
        _onDoOperation.Execute(operation);
    }
}