using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Flip : AbstractOperation
{
    private OperationTypes _type = OperationTypes.Flip;
    
    protected override async Task Do(OperationTypes type)
    {
        if (type != _type) return;
        
        transform.DORotate(new Vector3(0, 90, 0), _ctx.time);
        await Task.Delay((int) (_ctx.time * 1000));
    }
    
    public override OperationTypes GetOperationType =>
        _type;
}