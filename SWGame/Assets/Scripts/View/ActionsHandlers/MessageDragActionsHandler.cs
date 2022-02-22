using UnityEngine;
using UnityEngine.EventSystems;

namespace SWGame.View.ActionsHandlers
{
    public class MessageDragActionsHandler : MonoBehaviour, IDragHandler
    {
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }
    }
}
