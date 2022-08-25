using System.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Operations
{
    public class Drag : AbstractOperation
    {
        public new struct Ctx
        {
            public Camera camera;
            public ReactiveCommand<OperationTypes> onDoOperation;
            public ReactiveProperty<Vector3> mousePosition;
            public Vector3 extents;
            public float time;
        }
    
        private int LayerMask => UnityEngine.LayerMask.GetMask("Ground");
        private new Ctx _ctx;
        private bool _inProcess ;

        public void SetCtx(Ctx ctx)
        {
            _ctx = ctx;
            ctx.onDoOperation.Subscribe(Do).AddTo(this);
        }
    
        private OperationTypes _type = OperationTypes.Drag;
   
        public override OperationTypes GetOperationType =>
            _type;
    
        protected override async void Do(OperationTypes type)
        {
            if (_type != type || _inProcess)
                return;
        
            _inProcess = true;
        
            if (Physics.Raycast(_ctx.camera.ScreenPointToRay(_ctx.mousePosition.Value), out var hit, Mathf.Infinity, LayerMask))
            {
                // TODO count touch point offset and keep 
                transform.DOMove( hit.point + Vector3.up * _ctx.extents.y, _ctx.time);
            }
        
            await Task.Delay((int)(_ctx.time * 1000));
            _inProcess = false;
        }
    
        private void OnDisable()
        {
        }
    }
}