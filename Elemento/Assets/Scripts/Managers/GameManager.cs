using System.Collections.Generic;
using Assets.Scripts.Models;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public Game Game;
        public PrototypeManager PrototypeManager;


        public void Start()
        {
            //make the prototypeManager visible in unity inspector
            PrototypeManager = PrototypeManager.Instance;
            StartCoroutine(PrototypeManager.LoadPrototypes(NewGame));
        }

        public void NewGame()
        {
            // StartScreenPanel.SetActive(true);
            // GameOverPanel.SetActive(false);
            // GameWonPanel.SetActive(false);

            // DailyReportWindowController.Instance.gameObject.SetActive(false);
            // ProcessWindowConstroller.Instance.gameObject.SetActive(false);
            // ContactWindowController.Instance.gameObject.SetActive(false);
            // StartMenu.SetActive(false);
            // Notepad.SetActive(false);
            // ControlPanel.SetActive(false);

            Game = new Game();
            Game.Initialize();
        }

        public void EndGame(bool win)
        {
            Game.Paused = true;
            if (win)
            {
                // GameWonPanel.GetComponent<EndGamePanel>().Open();
            }
            else
            {
                // GameOverPanel.GetComponent<EndGamePanel>().Open();
            }
        }

        public void Update()
        {
            if (Game != null)
            {
                Game.Update(Time.deltaTime);
            }
        }
    }
}
