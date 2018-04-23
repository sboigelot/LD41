using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using Assets.Scripts.Framework;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Assets.Scripts.UI;
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

        public bool Collected;
        public float FloatUpOnCollectedTime = 0.5f;
        public float FloatUpOnCollectedSpeed = 35f;
        private float floatUpOnCollectedStart;

        public bool CollectOnMouseClick;

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

            var tooltip = gameObject.AddComponent<WorldTooltipProvider>();
            tooltip.content = "Wild element: " + ElementPrototype.Name + " <i>(click to collect)</i>";
        }

        public void FixedUpdate()
        {
            if (!Collected && 
                CollectOnMouseClick && 
                Input.GetMouseButton(0) && 
                IsThisUnderMouse())
            {
                GameManager.Instance.Game.Player.AddElement(Element);
                UiManager.Instance.ElementList.ReBuild();
                
                Collected = true;
                floatUpOnCollectedStart = Time.time;
            }

            if (Collected)
            {
                if (Time.time - floatUpOnCollectedStart >= FloatUpOnCollectedTime)
                {
                    Destroy(gameObject);
                }
                transform.position = transform.position + new Vector3(0, FloatUpOnCollectedSpeed * Time.deltaTime, 0);
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
