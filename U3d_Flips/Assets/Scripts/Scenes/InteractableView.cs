using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableView : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler,
    IDragHandler
{
    public struct Ctx
    {
        public ReactiveCommand onSelect;
        public ReactiveCommand<OperationTypes> onDoOperation;
        public List<OperationTypes> operations;
    }
    
    private Ctx _ctx;

    public void SetCtx(Ctx ctx)
    {
        _ctx = ctx;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"OnPointerClick on {name}");
        _ctx.onSelect.Execute();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"OnBeginDrag on {name}");
    }

    private void OnMouseUpAsButton()
    {
        OnInteract();
    }

    public virtual void OnInteract()
    {
        Debug.Log($"click on {name}");
        _ctx.onSelect.Execute();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"OnPointerDown on {name}");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"OnPointerUp on {name}");
    }

    public void OnMouseDown()
    {
        Debug.Log($"OnMouseDown on {name}");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log($"OnDrag on {name}");
    }
}