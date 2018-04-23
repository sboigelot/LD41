using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class TooltipController : MonoBehaviourSingleton<TooltipController>
    {
        public Text Text;
        public Vector3 LeftOffset;
        public Vector3 RigthOffset;
        public float TooltipDuration;

        public void Update()
        {
            transform.position = Input.mousePosition + GetOffset();
        }

        public void Show(string content)
        {
            Text.text = content;
            //Text.alignment = OnTheRigth() ? TextAnchor.MiddleRight: TextAnchor.MiddleLeft;
            transform.position = Input.mousePosition + GetOffset();
            gameObject.SetActive(true);
            //StartCoroutine(HideTooltip());
        }

        private Vector3 GetOffset()
        {
            if (OnTheRigth())
            {
                return RigthOffset;
            }
            return LeftOffset;
        }

        private bool OnTheRigth()
        {
            return Input.mousePosition.x > Screen.width / 2;
        }

        private IEnumerator HideTooltip()
        {
            yield return new WaitForSeconds(TooltipDuration);
            Hide();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}