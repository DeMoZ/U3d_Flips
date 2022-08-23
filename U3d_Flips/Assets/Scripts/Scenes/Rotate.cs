using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Rotate : AbstractOperation
{
    private OperationTypes _type = OperationTypes.Rotate;
    private bool _inProcess ;
   
    public override OperationTypes GetOperationType =>
        _type;
    protected override async void Do(OperationTypes type)
    {
        if (_type != type || _inProcess)
            return;
        
        Debug.Log($"[{this}]");

        _inProcess = true;
        var rotation = transform.rotation.eulerAngles;
        rotation += Vector3.up * 90;

        transform.DORotate(rotation, _ctx.time);
        await Task.Delay((int)(_ctx.time * 1000));
        _inProcess = false;
    }
    
    private void OnDisable()
    {
        _inProcess = false;
    }
}