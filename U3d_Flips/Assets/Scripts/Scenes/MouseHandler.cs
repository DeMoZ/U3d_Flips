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
        public List<InteractableEntity> interactables;
        //public ReactiveCommand<InteractableEntity> onSelectInteractable;
    }

    private Ctx _ctx;
    private List<IDisposable> _disposables;

    public MouseHandler(Ctx ctx)
    {
        _ctx = ctx;
        _disposables = new();

        var onMouseSelect = new ReactiveCommand<InteractableView>();
        var onMouseRelease = new ReactiveCommand();
        var onMouseDrag = new ReactiveCommand();
        var onMouseUp = new ReactiveCommand();
        onMouseSelect.Subscribe(OnMouseSelect).AddTo(_disposables);
        onMouseRelease.Subscribe().AddTo(_disposables);
        onMouseDrag.Subscribe().AddTo(_disposables);
        onMouseUp.Subscribe().AddTo(_disposables);
        
        var interactMousePm = new InteractiveMouse(new InteractiveMouse.Ctx
        {
            camera = _ctx.camera,
            mousePosition = _ctx.mousePosition,
            onMouseSelect = onMouseSelect,
            onMouseRelease = onMouseRelease,
            onMouseDrag = onMouseDrag,
            onMouseUp = onMouseUp,
        }).AddTo(_disposables);
    }

    private void OnMouseSelect(InteractableView view)
    {
        var interactable = _ctx.interactables.Find(i => i.View == view);

        //_ctx.onSelectInteractable.Execute(interactable);
        _ctx.current.Value = interactable;
        // if (_ctx.current.Value != null)
        // {
        //     // TODO unselect visual
        // }
    }
    
    private void OnInteractableSelected(InteractableEntity interactable)
    {
        if (_ctx.current.Value != null)
        {
            // TODO unselect visual
        }

        // if (_ctx.current.Value == interactable)
        // {
        //     _ctx.current.Value = null;
        //     _ctx.onSelectInteractable.Execute(new List<OperationTypes>());
        // }
        // else
        // {
        //     _ctx.current.Value = interactable;
        //     _ctx.onSelectInteractable.Execute(interactable.Data.operations);
        //     // TODO select (visual)
        // }
    }
    
    public void Dispose()
    {
        foreach (var d in _disposables)
            d.Dispose();
    }
}