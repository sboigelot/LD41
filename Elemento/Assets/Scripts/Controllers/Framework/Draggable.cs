using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Vector3 clickDivergence;

        public void OnBeginDrag(PointerEventData eventData)
        {
            clickDivergence = new Vector3(
                eventData.position.x - transform.position.x,
                eventData.position.y - transform.position.y);
        }

        public void OnDrag(PointerEventData eventData)
        {
            this.transform.position = new Vector3(
                eventData.position.x - clickDivergence.x,
                eventData.position.y - clickDivergence.y);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            
        }
    }
}