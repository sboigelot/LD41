using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class PrefabManager : MonoBehaviourSingleton<PrefabManager>
    {
        public GameObject TowerSlotPrefab;
        public GameObject TowerPrefab;
        public List<GameObject> TilePrefabs;
    }
}
