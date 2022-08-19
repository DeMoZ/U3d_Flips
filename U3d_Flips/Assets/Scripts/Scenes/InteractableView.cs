using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableView : MonoBehaviour, IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    public struct Ctx
    {
        public ReactiveCommand<(DragStates, PointerEventData)> onDragStates;
    }
    
    [SerializeField] private InteractableTypes type;
    
    private Ctx _ctx;

    public void SetCtx(Ctx ctx)
    {
        _ctx = ctx;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _ctx.onDragStates.Execute((DragStates.StartDrag, eventData));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _ctx.onDragStates.Execute((DragStates.EndDrag, eventData));
    }

    public void OnDrag(PointerEventData eventData)
    {
        _ctx.onDragStates.Execute((DragStates.ProcessDrag, eventData));
    }
}