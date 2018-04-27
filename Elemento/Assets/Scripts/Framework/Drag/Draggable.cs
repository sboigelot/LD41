using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static bool IsDragging;

        [HideInInspector]
        public Transform parentToReturnTo = null;

        [HideInInspector]
        public Transform placeHolderParent = null;

        private GameObject placeHolder = null;

        public Transform DefaultDragParent;

        public bool Allow3dWorld = true;

        public bool placeHolderHoldCopy;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (placeHolderHoldCopy)
            {
                placeHolder = Instantiate(gameObject);
            }
            else
            {
                placeHolder = new GameObject();
                LayoutElement le = placeHolder.AddComponent<LayoutElement>();
                le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
                le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
                le.flexibleWidth = 0;
                le.flexibleHeight = 0;
            }

            placeHolder.transform.SetParent(placeHolderParent);
            placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
            placeHolder.GetComponent<Draggable>().enabled = false;

            parentToReturnTo = this.transform.parent;
            placeHolderParent = parentToReturnTo;
            this.transform.SetParent(DefaultDragParent ?? this.transform.parent.parent);

            var cg = this.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.blocksRaycasts = false;
            }

            IsDragging = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            this.transform.position = eventData.position;

            //if (placeHolder.transform.parent != placeHolderParent)
            //{
            //    placeHolder.transform.SetParent(placeHolderParent);
            //    placeHolder.GetComponent<Draggable>().enabled = false;
            //}

            //int newSiblingIndex = placeHolderParent.childCount;

            //for (int i = 0; i < placeHolderParent.childCount; i++)
            //{
            //    if (this.transform.position.x < placeHolderParent.GetChild(i).position.x)
            //    {
            //        newSiblingIndex = i;

            //        if (placeHolder.transform.GetSiblingIndex() < newSiblingIndex)
            //            newSiblingIndex--;

            //        break;
            //    }
            //}

            //placeHolder.transform.SetSiblingIndex(newSiblingIndex);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            this.transform.SetParent(parentToReturnTo);
            this.transform.SetSiblingIndex(placeHolder == null ? 0 : placeHolder.transform.GetSiblingIndex());
            var cg = this.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.blocksRaycasts = true;
            }
            Destroy(placeHolder);
            IsDragging = false;

            if (!Allow3dWorld)
            {
                return;
            }

            var interactable = GetInteractableUnderMouse();
            if (interactable == null)
            {
                return;
            }

            var dropZone = interactable.GetComponent<DropZone>();
            if (dropZone != null)
            {
                dropZone.OnDrop(this);
            }
        }
        
        private GameObject GetInteractableUnderMouse()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (RaycastAll(ray, out hit))
            {
                return hit.collider.gameObject;
            }
            return null;
        }

        private bool RaycastAll(Ray ray, out RaycastHit hit)
        {
            // LayerMask layermask = new LayerMask {value = terrainGameObject.layer};

            hit = new RaycastHit();
            if (!Physics.Raycast(ray, out hit, 1000/*, layermask*/))
            {
                Debug.LogWarning("Raycast failed");
                return false;
            }
            return true;
        }
    }
}