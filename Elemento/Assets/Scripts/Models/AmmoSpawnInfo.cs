using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class AmmoSpawnInfo
    {
        public string PrefabName;
        public Vector3 Origin;
        public GameObject Target;
        public float Speed;
        public Dictionary<DamageType, int> Damage;
    }
}