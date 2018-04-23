using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers.UI
{
    public class DialogPanelController : MonoBehaviourSingleton<DialogPanelController>
    {
        public Image Image;
        public Text Text;
        public float TextSpeed;
        public string finalText;
        public int currentIndex;

        public Action OnNext;
        public Action OnSkip;

        private bool wasGamePaused;

        public void Open(Sprite sprite, string text, Action onNext, Action onSkip)
        {
            wasGamePaused = GameManager.Instance.Game.Paused;
            GameManager.Instance.Game.Paused = true;
            Image.sprite = sprite;
            Image.gameObject.SetActive(sprite != null);
            finalText = text;
            Text.text = "";
            currentIndex = 0;

            OnNext = onNext;
            OnSkip = onSkip;

            gameObject.SetActive(true);

            StartCoroutine(AddTextCoroutine());
        }

        public IEnumerator AddTextCoroutine()
        {
            while (currentIndex < finalText.Length)
            {
                Text.text += finalText[currentIndex];
                currentIndex++;
                yield return new WaitForSeconds(TextSpeed);
            }
        }

        public void DisplayAll()
        {
            currentIndex = finalText.Length;
            Text.text = finalText;
        }

        public void Skip()
        {
            GameManager.Instance.Game.Paused = wasGamePaused;
            gameObject.SetActive(false);
            if (OnSkip != null)
                OnSkip();
        }

        public void Next()
        {
            GameManager.Instance.Game.Paused = wasGamePaused;
            gameObject.SetActive(false);
            if (OnNext != null)
                OnNext();
        }
    }
}
