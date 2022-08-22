using System.Threading.Tasks;
using UnityEngine;

public abstract class AbstractOperation : MonoBehaviour
{
    public struct Ctx
    {
        public float time;
    }

    protected Ctx _ctx;

    public void SetCtx(Ctx ctx)
    {
        _ctx = ctx;
    }

    protected abstract Task Do(OperationTypes type);
    public abstract OperationTypes GetOperationType { get; }
}