using UniRx;
using UnityEngine;

public abstract class AbstractOperation : MonoBehaviour
{
    // it is Unnecessary for that class to be monoBehaviour
    // TODO can be refactored
    
    public struct Ctx
    {
        public float time;
        public ReactiveCommand<OperationTypes> onDoOperation;
    }

    protected Ctx _ctx;

    public void SetCtx(Ctx ctx)
    {
        _ctx = ctx;
        _ctx.onDoOperation.Subscribe(Do).AddTo(this);
    }

    protected abstract void Do(OperationTypes type);
    public abstract OperationTypes GetOperationType { get; }

    private void OnDisable()
    {
    }
}