﻿using System;
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
        public bool Paused = true;

        public Player Player;
        public Level CurrentLevel;
        
        public void Initialize()
        {
            Paused = false;
        }

        public void Update(float deltaTime)
        {
            if (!Paused && PrototypeManager.Instance.Loaded)
            {
                GameTime += Time.deltaTime;
            }
        }      
    }
}