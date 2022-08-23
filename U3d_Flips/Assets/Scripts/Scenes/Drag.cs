using UniRx;
using UnityEngine;

public class Drag : AbstractOperation
{
    public new struct Ctx
    {
        public Camera camera;
        public ReactiveCommand<OperationTypes> onDoOperation;
        public ReactiveProperty<Vector3> mousePosition;
        public Vector3 extents;
    }
    
    private int LayerMask => UnityEngine.LayerMask.GetMask("Ground");
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
        
        if (Physics.Raycast(_ctx.camera.ScreenPointToRay(_ctx.mousePosition.Value), out var hit, Mathf.Infinity, LayerMask))
        {
            // TODO lerp position and/or count touch point offset and keep 
            transform.position = hit.point + Vector3.up * _ctx.extents.y;
        }
    }
    
    private void OnDisable()
    {
    }
}