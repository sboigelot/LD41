using System;
using System.Linq;
using Assets.Scripts.Managers;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Game
    {
        public float GameTime;
        private bool paused = true;

        public int GameSpeed = 1;

        public Player Player;
        public Level CurrentLevel;

        public bool Paused
        {
            get
            {
                return paused;
            }
            set
            {
                paused = value;
                UiManager.Instance.PauseText.SetActive(value);
            }
        }

        public void Initialize()
        {
            Paused = false;
        }

        public void Update(float deltaTime)
        {
            if (!Paused && PrototypeManager.Instance.Loaded && CurrentLevel != null)
            {
                GameTime += Time.deltaTime * GameSpeed;
                CurrentLevel.Update(deltaTime);
            }
        }      
    }
}