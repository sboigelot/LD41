using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using Assets.Scripts.Framework;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class FloatingElementController : Spin
    {
        public Action<Element> OnMouseOver;

        public Element Element;
        public ElementPrototype ElementPrototype;

        public MeshRenderer FrontFace;
        public MeshRenderer BackFace;

        public bool CollectOnMouseOver;

        public void Build()
        {
            if (Element == null)
            {
                Destroy(gameObject);
            }

            if (ElementPrototype == null)
            {
                ElementPrototype = PrototypeManager.Instance.GetPrototype<ElementPrototype>(Element.Uri);
            }

            var sprite = SpriteManager.Instance.GetChached("Images/Elements", ElementPrototype.SpritePath);
            FrontFace.material.SetTexture("_MainTex", sprite.texture);
        }

        public void FixedUpdate()
        {
            if (CollectOnMouseOver && IsThisUnderMouse())
            {
                GameManager.Instance.Game.Player.AddElement(Element);
                UiManager.Instance.ElementList.ReBuild();
                Destroy(gameObject);
            }
        }

        private bool IsThisUnderMouse()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (RaycastAll(ray, out hit))
            {
                return hit.collider.gameObject == gameObject;
            }
            return false;
        }

        private bool RaycastAll(Ray ray, out RaycastHit hit)
        {
            // LayerMask layermask = new LayerMask {value = terrainGameObject.layer};

            hit = new RaycastHit();
            if (!Physics.Raycast(ray, out hit, 1000/*, layermask*/))
            {
                //Debug.LogWarning("Raycast failed");
                return false;
            }
            return true;
        }
    }
}
