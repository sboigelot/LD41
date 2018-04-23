using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.UI.Controls.ContextualMenu;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

namespace Assets.Scripts
{
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("UI/Pyralis/ContextualMenu")]
    public class ContextualMenu : MonoBehaviour, IPointerExitHandler
    {
        public List<ContextualMenuItemInfo> ContextualMenuItemInfos;
        public GameObject Instanciator;
        private Transform itemPanel;
        private GameObject itemTemplate;
        public Action OnClose;
        public GameObject OverrideProvider;
        public AnimationCurve FdAnimationCurve;
        public float EstimatedMaxChildren;

        public void OnPointerExit(PointerEventData eventData)
        {
            gameObject.SetActive(false);
            if (OnClose != null)
            {
                OnClose.Invoke();
            }
        }

        public void Start()
        {
            var rectTransform = GetComponent<RectTransform>();
            itemTemplate = rectTransform.Find("ItemTemplate").gameObject;
            itemTemplate.SetActive(false);
            itemPanel = rectTransform.Find("ItemPanel").gameObject.transform;
            itemPanel.gameObject.SetActive(true);
        }

        public void Open(GameObject instanciator, PointerEventData eventData, Vector3 position, Action onClose)
        {
            Instanciator = instanciator;
            OnClose = onClose;
            gameObject.SetActive(false);

            if (OverrideProvider != null)
            {
                var overrideInterface = OverrideProvider.GetComponent<IContextualMenuItemInfoProvider>();
                ContextualMenuItemInfos = overrideInterface.GetContextualMenuInfo().ToList();
            }
            else
            {
                ScanForContextualMenuItemInfoProvider(eventData);
            }

            RebuildChildren();
            transform.position = position;

            float fdeval = (float)itemPanel.transform.childCount / EstimatedMaxChildren;
            itemPanel.GetComponent<RadialLayout>().FDistanceMultiplier = FdAnimationCurve.Evaluate(fdeval);

            gameObject.SetActive(true);

            transform.SetAsLastSibling();
        }

        private void ScanForContextualMenuItemInfoProvider(PointerEventData eventData)
        {
            var gameObjectsUnderMouse = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, gameObjectsUnderMouse);
            ContextualMenuItemInfos = new List<ContextualMenuItemInfo>();

            if (gameObjectsUnderMouse.Count == 0)
            {
                return;
            }

            var firstGameObjectUnderMouse = gameObjectsUnderMouse.FirstOrDefault();
            if (firstGameObjectUnderMouse.gameObject != null)
            {
                var components =
                    firstGameObjectUnderMouse.gameObject.GetComponents(typeof(IContextualMenuItemInfoProvider));
                var contextualItems =
                    components.SelectMany(c => ((IContextualMenuItemInfoProvider) c).GetContextualMenuInfo()).ToList();
                ContextualMenuItemInfos.AddRange(contextualItems);
            }
        }

        private void RebuildChildren()
        {
            itemPanel.ClearChildren();

            if (ContextualMenuItemInfos == null)
            {
                return;
            }

            foreach (var info in ContextualMenuItemInfos)
            {
                var newItem = Instantiate(itemTemplate);
                newItem.name = "Item " + info.Name;
                newItem.transform.SetParent(itemPanel, false);
                newItem.SetActive(true);
                var menuItem = newItem.AddComponent<ContextualMenuItem>();
                menuItem.Initialize(this, info);
            }
        }
    }
}