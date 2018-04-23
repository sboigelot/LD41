using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Assets.Scripts.Managers;
using Assets.Scripts.Managers.DialogBoxes;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers.UI
{
    public class ConfirmationMessageController : ContextualDialogBoxBase<ConfirmationMessageController, ConfirmationContext>
    {
        public Text ContenText;

        protected override void OnScreenOpen(ConfirmationContext context)
        {
            GameManager.Instance.Game.Paused = true;
        }

        protected override void OnScreenClose(ConfirmationContext context)
        {
            GameManager.Instance.Game.Paused = false;
        }
    }

    public class ConfirmationContext
    {
        
    }
}
