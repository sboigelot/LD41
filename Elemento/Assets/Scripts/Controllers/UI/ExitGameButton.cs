using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Controllers.UI
{
    public class ExitGameButton : MonoBehaviour
    {
        public void ExitGame()
        {
            Application.Quit(); 
        }
    }
}
