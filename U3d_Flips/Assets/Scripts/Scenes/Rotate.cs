using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Rotate : AbstractOperation
{
    private OperationTypes _type = OperationTypes.Rotate;

    protected override async void Do(OperationTypes type)
    {
        if (type != _type) return;

        transform.DORotate(new Vector3(0, 90, 0), _ctx.time);
        await Task.Delay((int) (_ctx.time * 1000));
    }

    public override OperationTypes GetOperationType =>
        _type;
}