namespace Assets.Scripts.Managers
{
    public class MainMenuManager : MonoBehaviourSingleton<MainMenuManager>
    {
        public PrototypeManager PrototypeManager;

        public LevelSelectList LevelSelectList;

        public void Start()
        {
            //make the prototypeManager visible in unity inspector
            PrototypeManager = PrototypeManager.Instance;
            StartCoroutine(PrototypeManager.LoadPrototypes(PopulateLevels, protoType => protoType == "level"));
        }

        private void PopulateLevels()
        {
            LevelSelectList.ReBuild();
        }
    }
}