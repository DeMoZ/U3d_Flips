using System;
using UniRx;
using UnityEngine;

public class InteractableView : MonoBehaviour, IDisposable
{
    public struct Ctx
    {
        public Texture2D texture;
        public ReactiveCommand<bool> onColorChange;
    }

    [SerializeField] private Rigidbody _rigidbody = default;
    [SerializeField] private Renderer _renderer = default;

    private Ctx _ctx;
    private CompositeDisposable _disposables;
    public void SetCtx(Ctx ctx)
    {
        _ctx = ctx;
        _disposables = new CompositeDisposable();
        
        var material = _renderer.material;
        material.mainTexture = _ctx.texture;

        _ctx.onColorChange.Subscribe(isSelected =>
        {
            material.color = isSelected ? Color.gray : Color.white;
            _rigidbody.isKinematic = isSelected;
        }).AddTo(_disposables);
        
    }
    
    public void Dispose()
    {
        _disposables.Dispose();
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