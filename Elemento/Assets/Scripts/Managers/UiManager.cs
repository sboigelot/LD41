using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Controllers.Game.UI;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class UiManager : MonoBehaviourSingleton<UiManager>
    {
        public ElementListController ElementList;
        public GameObject PauseText;
    }
}


