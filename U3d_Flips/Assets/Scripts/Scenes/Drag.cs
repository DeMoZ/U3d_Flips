using UniRx;
using UnityEngine;

public class Drag : AbstractOperation
{
    public new struct Ctx
    {
        public float time;
        public ReactiveCommand<OperationTypes> onDoOperation;
        public ReactiveProperty<Vector3> mousePosition;
    }
    
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
        
        Debug.Log($"[{this}]");

       
        
    }
    
    private void OnDisable()
    {
    }
}