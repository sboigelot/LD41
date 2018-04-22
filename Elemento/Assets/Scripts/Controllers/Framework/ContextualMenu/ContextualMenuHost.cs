using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.UI.Controls.ContextualMenu;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Controllers.Framework.ContextualMenu
{
    public class ContextualMenuHost : MonoBehaviour, IContextualMenuItemInfoProvider, IPointerDownHandler, IPointerUpHandler
    {
        public Scripts.ContextualMenu Menu;
        private PointerEventData pressEventData;
        private Vector2 pressStartPosition;
        private float pressStart = float.MaxValue;
        private float pressActionThreshold = .4f;

        public Sprite DefaultItemSprite;
        
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
                if (Menu != null)
                {
                    Menu.Open(gameObject, pressEventData, pressEventData.position, null);
                }
            }
        }
        
        public virtual IEnumerable<ContextualMenuItemInfo> GetContextualMenuInfo()
        {
            yield return new ContextualMenuItemInfo
            {
                Image = DefaultItemSprite,
                IsEnable = () => true,
                Name = "Item 1",
                TooltipText = "Item 1",
                OnClick = LogClick
            };

            yield return new ContextualMenuItemInfo
            {
                Image = DefaultItemSprite,
                IsEnable = () => true,
                Name = "Item 2",
                TooltipText = "Item 2",
                OnClick = LogClick
            };

            yield return new ContextualMenuItemInfo
            {
                Image = DefaultItemSprite,
                IsEnable = () => true,
                Name = "Item 3",
                TooltipText = "Item 3",
                OnClick = LogClick
            };
        }

        private void LogClick(Scripts.ContextualMenu menu, GameObject instanciator, Vector3 position)
        {
            Debug.Log("click on ctx menu item");
        }
    }
}
