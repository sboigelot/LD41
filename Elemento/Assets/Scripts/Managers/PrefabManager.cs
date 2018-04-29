using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class PrefabManager : MonoBehaviourSingleton<PrefabManager>
    {
        public List<GameObject> TilePrefabs;

        public Material ProgressMaterial;

        [Serializable]
        public class PrefabInfo
        {
            public string Name;

            public GameObject Prefab;
        }

        public List<PrefabInfo> Prefabs;

        public GameObject GetPrefab(string prefabName)
        {
            var prefabInfo = Prefabs.FirstOrDefault(pi => pi.Name == prefabName);

            if (prefabInfo == null)
            {
                Debug.LogError("Coudn't find PrefabInfo named: "+prefabName);
                return null;
            }

            return prefabInfo.Prefab;
        }
    }
}
