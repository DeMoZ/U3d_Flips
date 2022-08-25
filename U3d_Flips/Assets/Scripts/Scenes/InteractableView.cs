using UnityEngine;

public class InteractableView : MonoBehaviour
{
    public struct Ctx
    {
        public Texture2D texture;
    }
    
    private Ctx _ctx;

    public void SetCtx(Ctx ctx)
    {
        _ctx = ctx;

        var material = GetComponent<Renderer>().material;
        material.mainTexture = _ctx.texture;
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