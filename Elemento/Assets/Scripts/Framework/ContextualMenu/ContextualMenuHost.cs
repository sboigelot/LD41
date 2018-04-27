using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Controllers.Framework.ContextualMenu
{
    public class ContextualMenuHost : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Scripts.ContextualMenu Menu;
        private PointerEventData pressEventData;
        private Vector2 pressStartPosition;
        private float pressStart = float.MaxValue;
        private float pressActionThreshold = .15f;
        public Vector3 prevPosition;

        public void OnPointerDown(PointerEventData eventData)
        {
            pressStartPosition = Input.mousePosition;
            pressStart = Time.time;
            pressEventData = eventData;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            pressStart = float.MaxValue;
        }

        public void Update()
        {
            if (Time.time - pressStart > pressActionThreshold &&
                pressStartPosition.x == Input.mousePosition.x &&
                pressStartPosition.y == Input.mousePosition.y)
            {
                pressStart = float.MaxValue;
                prevPosition = pressEventData.position;
                if (Menu != null)
                {
                    Menu.Open(gameObject, pressEventData, pressEventData.position, null, false);
                }
            }
        }

        public void ReOpen()
        {
            if (Menu != null)
            {
                Menu.Open(gameObject, pressEventData, prevPosition, null, true);
                Menu.gameObject.SetActive(true);
            }
        }
    }
}
