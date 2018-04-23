using Assets.Scripts.Models;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class LoadLevelButton : LoadSceneButton
    {
        public Image Image;
        public Text Title;
        public Level Level;

        public void Awake()
        {
            StartCoroutine(SpriteManager.Set(Image, "Data/Levels", Level.PreviewPicPath));
        }

        public void Build(Level level)
        {
            Level = level;
            Title.text = level.Name;
        }

        public void Load()
        {
            FindObjectOfType<LevelDataHolder>().Level = Level;
            LoadScene("Scenes/Game Level");
        }
    }
}