using UniRx;
using UnityEngine;

public abstract class AbstractOperation : MonoBehaviour
{
    public struct Ctx
    {
        public float time;
        public ReactiveCommand<OperationTypes> onDoOperation;
    }

    protected Ctx _ctx;

    public void SetCtx(Ctx ctx)
    {
        _ctx = ctx;
        _ctx.onDoOperation.Subscribe(Do);
    }

    protected abstract void Do(OperationTypes type);
    public abstract OperationTypes GetOperationType { get; }
}