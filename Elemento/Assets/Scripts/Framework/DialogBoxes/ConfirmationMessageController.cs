using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Assets.Scripts.Managers.DialogBoxes;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers.UI
{
    public abstract class ConfirmationMessageController<T> : ContextualDialogBoxBase<T, ConfirmationContext>, IDialogBox where T : new()
    {
        public Text ContentText;

        public void Yes()
        {
            if (Context == null)
                Context = new ConfirmationContext();
            Context.Result = true;
            CloseDialog();
        }

        public void No()
        {
            if (Context == null)
                Context = new ConfirmationContext();
            Context.Result = false;
            CloseDialog();
        }
    }

    public class ConfirmationContext
    {
        public bool Result;
    }
}
