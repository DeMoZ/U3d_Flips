using UniRx;
using UnityEngine;

public class InteractableView : MonoBehaviour
{
    public struct Ctx
    {
        // public ReactiveCommand onSelect;
        // public ReactiveCommand onRelease;

        //public ReactiveCommand<MouseStates> onMouseStates;
    }
    
    private Ctx _ctx;

    public void SetCtx(Ctx ctx)
    {
        _ctx = ctx;
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