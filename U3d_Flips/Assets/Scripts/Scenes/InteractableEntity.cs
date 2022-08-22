using System;
using System.Collections.Generic;
using System.Linq;

public class InteractableEntity
{
    public struct Ctx
    {
        public OperationsSet operationsSet;
        public InteractableView view;
        public InteractableTypes type;
        public List<OperationTypes> operations;
    }

    private Ctx _ctx;

    private List<AbstractOperation> _operations;

    public Ctx Data => _ctx;

    public InteractableEntity(Ctx ctx)
    {
        _ctx = ctx;
        
        _operations = new();
        _operations.AddRange(_ctx.view.gameObject.GetComponents<AbstractOperation>());

        foreach (var ctxOperation in _ctx.operations)
        {
            var o = _operations.FirstOrDefault(o => o.GetOperationType == ctxOperation);
            if (o == null)
                o = AddOperation(ctxOperation);
        }
    }

    private AbstractOperation AddOperation(OperationTypes type)
    {
        switch (type)
        {
            case OperationTypes.Flip:
                var flip = _ctx.view.gameObject.AddComponent<Flip>();
                flip.SetCtx(new AbstractOperation.Ctx
                {
                    time = 0.5f,
                });
                _operations.Add(flip);
                return flip;
                break;
            case OperationTypes.Rotate:
                var rotate = _ctx.view.gameObject.AddComponent<Rotate>();
                rotate.SetCtx(new AbstractOperation.Ctx
                {
                    time = 0.5f,
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
        
    }
}