﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class Tower
    {
        [XmlAttribute]
        public bool IsStronghold;

        [XmlAttribute]
        public string BaseElementUri;

        [XmlAttribute]
        public string BodyElementUri;

        [XmlAttribute]
        public string WeaponElementUri;

        [XmlAttribute]
        public string EnchantElementUri;
        
        [XmlIgnore]
        public float LastShot;

        public Dictionary<DamageType, float> GetDamage()
        {
            var dictionary = new Dictionary<DamageType, float>();

            Aggregate(s =>
            {
                if (!dictionary.ContainsKey(s.DamageType))
                {
                    dictionary.Add(s.DamageType, 0);
                }

                var newValue = dictionary[s.DamageType] + s.DamageAmount;
                if (newValue < 0)
                    newValue = 0;
                dictionary[s.DamageType] = newValue;
            });

            return dictionary;
        }

        public float GetSpeed()
        {
            float value = 8f;
            Aggregate(s => value -= (s.SpeedBonus*1f));
            return value >= 0.1f ? value : 0.1f;
        }

        public float GetRange()
        {
            float value = 2.5f;
            Aggregate(s => value += (s.RangeBonus*1f));
            return value;
        }

        public float GetAmmoSpeed()
        {
            float value = .5f;
            Aggregate(s => value += s.AmmoSpeedBonus);
            return value;
        }

        private void Aggregate(Action<ElementPrototypeStat> onStat)
        {
            AggregateSingleElement(onStat, BaseElementUri, TowerSlotType.Base);
            AggregateSingleElement(onStat, BodyElementUri, TowerSlotType.Body);
            AggregateSingleElement(onStat, WeaponElementUri, TowerSlotType.Weapon);
            AggregateSingleElement(onStat, EnchantElementUri, TowerSlotType.Enchant);
        }

        public bool ReadyToShoot
        {
            get { return LastShot < GameManager.Instance.Game.GameTime - GetSpeed(); }
        }

        private static void AggregateSingleElement(Action<ElementPrototypeStat> onStat, string uri, TowerSlotType slot)
        {
            if (!string.IsNullOrEmpty(uri))
            {
                var prototype = PrototypeManager.Instance.GetPrototype<ElementPrototype>(uri);

                if (prototype != null)
                {
                    foreach (var elementPrototypeStat in prototype.ElementStats.Where(s => s.InSlot == slot))
                    {
                        onStat(elementPrototypeStat);
                    }
                }
            }
        }
        
        public GameObject FindTarget(Vector3 worldPosition)
        {
            var range = GetRange();
            return GameManager.Instance.CurrentMonsters.FirstOrDefault(m => MonsterIsInRange(range, worldPosition, m));
        }

        public AmmoSpawnInfo ShootAtTarget(GameObject ennemyInRange)
        {
            LastShot = GameManager.Instance.Game.GameTime;
            var prototype = PrototypeManager.Instance.GetPrototype<ElementPrototype>(WeaponElementUri);
            var stat = prototype.ElementStats.First(s => s.InSlot == TowerSlotType.Weapon);
            return new AmmoSpawnInfo
            {
                Damage = GetDamage(),
                Target = ennemyInRange,
                Speed = GetAmmoSpeed(),
                PrefabName = stat.AmmoPrefabName
            };
        }

        private bool MonsterIsInRange(float range, Vector3 worldPosition, GameObject monster)
        {
            if (monster == null)
            {
                return false;
            }

            var distance = Vector3.Distance(monster.transform.position, worldPosition);
            return distance <= range;
        }
    }
}