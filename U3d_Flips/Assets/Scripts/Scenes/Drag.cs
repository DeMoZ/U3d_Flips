using UniRx;
using UnityEngine;

public class Drag : AbstractOperation
{
    public new struct Ctx
    {
        public Camera camera;
        public ReactiveCommand<OperationTypes> onDoOperation;
        public ReactiveProperty<Vector3> mousePosition;
    }
    
    private int _layerMask =>LayerMask.GetMask("Ground");
    private new Ctx _ctx;
    
    public void SetCtx(Ctx ctx)
    {
        _ctx = ctx;
        ctx.onDoOperation.Subscribe(Do).AddTo(this);
    }
    
    private OperationTypes _type = OperationTypes.Drag;
   
    public override OperationTypes GetOperationType =>
        _type;
    
    protected override void Do(OperationTypes type)
    {
        if (_type != type)
            return;
        
        if (Physics.Raycast(_ctx.camera.ScreenPointToRay(Input.mousePosition), out var hit, Mathf.Infinity, _layerMask))
        {
            transform.position = hit.point;
        }
    }
    
    private void OnDisable()
    {
    }
}