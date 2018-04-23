using Assets.Scripts.Managers;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Controllers.UI
{
    public class QuitDialogController : ConfirmationMessageController<QuitDialogController>
    {
        protected override void OnScreenOpen(ConfirmationContext context)
        {
            GameManager.Instance.Game.Paused = true;
        }

        protected override void OnScreenClose(ConfirmationContext context)
        {
            GameManager.Instance.Game.Paused = false;
            if (context.Result)
            {
                SceneManager.LoadScene("Scenes/Main Menu", LoadSceneMode.Single);
            }
        }
    }
}