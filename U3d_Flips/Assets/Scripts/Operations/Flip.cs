using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Operations
{
    public class Flip : AbstractOperation
    {
        private OperationTypes _type = OperationTypes.Flip;
        private bool _inProcess ;
   
        public override OperationTypes GetOperationType =>
            _type;
        protected override async void Do(OperationTypes type)
        {
            if (_type != type || _inProcess)
                return;
        
            Debug.Log($"[{this}]");

            _inProcess = true;
       
            var rotation = transform.rotation;
            rotation = Quaternion.Euler(Vector3.right * 180) * rotation;

            transform.DORotate(rotation.eulerAngles, _ctx.time);
            await Task.Delay((int)(_ctx.time * 1000));
            _inProcess = false;
        }
    
        private void OnDisable()
        {
            _inProcess = false;
        }
    }
}