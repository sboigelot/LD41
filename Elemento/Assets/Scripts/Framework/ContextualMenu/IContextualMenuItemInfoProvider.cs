using System.Collections.Generic;

namespace Assets.Scripts.UI.Controls.ContextualMenu
{
    public interface IContextualMenuItemInfoProvider
    {
        IEnumerable<ContextualMenuItemInfo> GetContextualMenuInfo(bool reOpen);
    }
}