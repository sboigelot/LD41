using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class ContextualMenuItemInfo
    {
        public Sprite Image;
        public Func<bool> IsEnable;
        public string Name;
        public Action<ContextualMenu, GameObject, Vector3> OnClick;
        public string TooltipText;
    }
}