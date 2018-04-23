using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class LevelDataHolder : MonoBehaviour
    {
        public Level Level;

        public void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}