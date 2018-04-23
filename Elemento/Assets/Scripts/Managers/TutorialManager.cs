using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Assets.Scripts.Controllers.UI;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class TutorialManager : MonoBehaviourSingleton<TutorialManager>
    {
        public Sprite JanitorSprite;
        public Sprite BookSprite;
        private Level level;

        public void OpenJanitorDialog(string text)
        {
            DialogPanelController.Instance.Open(JanitorSprite, text, 
                () => { level.TutorialIndex++; },
                () => { level.TutorialIndex = int.MaxValue; });
        }

        public void OpenBookDialog(string text)
        {
            DialogPanelController.Instance.Open(BookSprite, text,
                () => { level.TutorialIndex++; },
                () => { level.TutorialIndex = int.MaxValue; });
        }

        public void UpdateTutorial(Level l)
        {
            level = l;

            if (level.TutorialSteps == null)
            {
                return;
            }

            var nextDialog = level.TutorialSteps.FirstOrDefault(s => s.Index > level.TutorialIndex);
            if (nextDialog == null)
            {
                return;
            }

            if (nextDialog.StartTime > GameManager.Instance.Game.GameTime)
            {
                return;
            }

            if (nextDialog.IsJanitor)
            {
                OpenJanitorDialog(nextDialog.Text);
            }

            if (nextDialog.IsBook)
            {
                OpenBookDialog(nextDialog.Text);
            }
        }
    }
}
