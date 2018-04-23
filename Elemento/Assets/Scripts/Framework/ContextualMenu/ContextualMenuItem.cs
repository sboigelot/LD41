using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class ContextualMenuItem : MonoBehaviour
    {
        public ContextualMenuItemInfo Info;
        private ContextualMenu menu;

        public void Initialize(ContextualMenu menu, ContextualMenuItemInfo info)
        {
            this.menu = menu;
            Info = info;

            var image = GetComponent<Image>() ?? GetComponentInChildren<Image>();
            if (image != null && Info != null)
            {
                image.sprite = Info.Image;
            }

            // GetComponentInChildren<Text>().text = Info.TooltipText;
            var button = GetComponent<Button>() ?? GetComponentInChildren<Button>();
            if (button != null && Info != null)
            {
                button.onClick.AddListener(() => OnButtonClick());
            }

            var tooltip = GetComponent<TooltipProvider>() ?? GetComponentInChildren<TooltipProvider>();
            if (tooltip != null && info != null)
            {
                tooltip.content = info.TooltipText;
            }

            button.enabled = Info == null || Info.IsEnable == null || Info.IsEnable();
        }

        public void OnButtonClick()
        {
            if (Info != null && Info.OnClick != null)
            {
                menu.gameObject.SetActive(false);
                Info.OnClick.Invoke(menu, menu.Instanciator, transform.parent.position);
            }
        }
    }
}