using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class InteractiveMouse : IDisposable
{
    public struct Ctx
    {
        public Camera camera;
        public ReactiveProperty<Vector3> mousePosition;
        public ReactiveCommand<InteractableView> onMouseSelect;
        public ReactiveCommand onMouseDrag;
        public ReactiveCommand onMouseUp;
    }

    private Ctx _ctx;
    private List<IDisposable> _disposables;
    private Vector3? _previous;
    
    public InteractiveMouse(Ctx ctx)
    {
        _ctx = ctx;
        _disposables = new();
        // all the time
        /*Observable.EveryUpdate().Subscribe(_ =>
        {
            mousePosition.Value = Input.mousePosition;
            Debug.Log($"[mouse] {Input.mousePosition}");
        });*/
        
        // on mouse down moment
        Observable.EveryUpdate().Where(_=>Input.GetMouseButtonDown(0)).Subscribe(_ =>
        {
            Debug.Log($"[InteractMousePm][GetMouseButtonDown] {Input.mousePosition}");
            OnMomentMouseDown();
        }).AddTo(_disposables);
        
        // while mouse down
        Observable.EveryUpdate().Where(_=>Input.GetMouseButton(0)).Subscribe(_ =>
        {
            Debug.Log($"[InteractMousePm][GetMouseButton] {Input.mousePosition}");
            OnWhileMouseDown();
        }).AddTo(_disposables);
        
        // on mouse up moment
        Observable.EveryUpdate().Where(_=>Input.GetMouseButtonUp(0)).Subscribe(_ =>
        {
            Debug.Log($"[InteractMousePm][GetMouseButtonUp] {Input.mousePosition}");
            OnMomentMouseUp();
        }).AddTo(_disposables);
    }

    private void OnMomentMouseDown()
    {
        _previous = Input.mousePosition;
        _ctx.mousePosition.Value = Input.mousePosition;
        
        RaycastHit hit;
        if (Physics.Raycast(_ctx.camera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (hit.transform.TryGetComponent<InteractableView>(out var view))
            {
                _ctx.onMouseSelect.Execute(view);
                return;
            }
        }
        
        _ctx.onMouseSelect.Execute(null);
    }

    private void OnWhileMouseDown()
    {
        if (_previous.HasValue && (_previous.Value - Input.mousePosition).magnitude < 0.001f)
            return;
        
        _previous = Input.mousePosition;

        _ctx.mousePosition.Value = Input.mousePosition;
        _ctx.onMouseDrag.Execute();
    }
    
    private void OnMomentMouseUp()
    {
        _previous = null;
        _ctx.mousePosition.Value = Input.mousePosition;
        _ctx.onMouseUp.Execute();
    }

    public void Dispose()
    {
        foreach (var d in _disposables) 
            d.Dispose();
    }
}