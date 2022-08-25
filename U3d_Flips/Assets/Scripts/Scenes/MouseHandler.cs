using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MouseHandler : IDisposable
{
    public struct Ctx
    {
        public Camera camera;
        public ReactiveProperty<InteractableEntity> current;
        public ReactiveProperty<Vector3> mousePosition;
        public ReactiveCommand onDragObject;
        public List<InteractableEntity> interactables;
    }

    private Ctx _ctx;
    private CompositeDisposable _disposables;
    private Vector3? _startPosition;
    private bool _repeatSelect;

    public MouseHandler(Ctx ctx)
    {
        _ctx = ctx;
        _disposables = new();

        var onMouseSelect = new ReactiveCommand<InteractableView>();
        var onMouseDrag = new ReactiveCommand();
        var onMouseUp = new ReactiveCommand();
        onMouseSelect.Subscribe(OnMouseSelect).AddTo(_disposables);
        onMouseDrag.Subscribe(_ => OnMouseDrag()).AddTo(_disposables);
        onMouseUp.Subscribe(_ => OnMouseUp()).AddTo(_disposables);

        var interactMousePm = new InteractiveMouse(new InteractiveMouse.Ctx
        {
            camera = _ctx.camera,
            mousePosition = _ctx.mousePosition,
            onMouseSelect = onMouseSelect,
            onMouseDrag = onMouseDrag,
            onMouseUp = onMouseUp,
        }).AddTo(_disposables);
    }

    private void OnMouseSelect(InteractableView view)
    {
        var interactable = _ctx.interactables.Find(i => i.View == view);
        _startPosition = _ctx.mousePosition.Value;

        if (_ctx.current.Value == interactable)
            _repeatSelect = true;

        _ctx.current.Value = interactable;
    }

    private void OnMouseDrag()
    {
        _startPosition = null;
        _repeatSelect = false;
        
        if (_ctx.current.Value != null)
            _ctx.onDragObject.Execute();
    }

    private void OnMouseUp()
    {
        if (_startPosition.HasValue && _startPosition.Value == _ctx.mousePosition.Value)
        {
            if (_repeatSelect) // release if click on selected before
            {
                _ctx.current.Value = null;
                _repeatSelect = false;
            }
        }
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}