using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Controllers.Framework;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class LevelSelectList : UiItemList<Level>
    {
        protected override List<Level> GetData()
        {
            return MainMenuManager.Instance.PrototypeManager.GetAllPrototypes<Level>().ToList();
        }

        protected override void Prepare(GameObject itemObject, Level data)
        {
            var LoadLevelButton = itemObject.GetComponentInChildren<LoadLevelButton>();
            LoadLevelButton.Build(data);
        }
    }
}