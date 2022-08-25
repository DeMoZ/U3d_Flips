using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class InteractableView : MonoBehaviour, IDisposable
{
    public struct Ctx
    {
        public Texture2D texture;
        public ReactiveCommand<bool> onColorChange;
    }
    
    private Ctx _ctx;
    private List<IDisposable> _disposables;
    public void SetCtx(Ctx ctx)
    {
        _ctx = ctx;
        _disposables = new List<IDisposable>();
        
        var rigidbody = GetComponent<Rigidbody>();
        var material = GetComponent<Renderer>().material;
        material.mainTexture = _ctx.texture;

        
        _ctx.onColorChange.Subscribe(isSelected =>
        {
            material.color = isSelected ? Color.gray : Color.white;
            rigidbody.isKinematic = isSelected;
        }).AddTo(_disposables);
        
    }
    
    public void Dispose()
    {
        foreach (var disposable in _disposables) 
            disposable?.Dispose();
    }
    
    /*private void OnMouseDown()
    {
        Debug.Log($"OnMouseDown on {name}");
        //_ctx.onSelect.Execute();
        _ctx.onMouseStates.Execute(MouseStates.Down);
    }

    private void OnMouseDrag()
    {
        Debug.Log($"OnMouseDrag");
        _ctx.onMouseStates.Execute(MouseStates.Down);
    }
    
    private void OnMouseUpAsButton()
    {
        Debug.Log($"OnMouseUpAsButton");
        //_ctx.onRelease.Execute();
        _ctx.onMouseStates.Execute(MouseStates.Up);
    }

    private void OnMouseUp()
    {
        Debug.Log($"OnMouseUp");
        _ctx.onMouseStates.Execute(MouseStates.Up);
    }*/
}