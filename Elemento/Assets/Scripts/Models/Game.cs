using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
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

    [Serializable]
    public class ProtoypeIndex
    {
        [XmlAttribute]
        public string Uri;

        [XmlIgnore]
        public string ProtoypeType
        {
            get { return !string.IsNullOrEmpty(Uri) ? Uri.Split(':')[0] : "unset"; }
        }

        [XmlAttribute]
        public string Path;
    }
    
    public interface Prototype
    {
        
    }

    [Serializable]
    public class Player : Prototype
    {
        [XmlElement("UnlockedTowerTemplate")]
        public List<string> UnlockedTowerTemplates;

        public List<Element> Elements;

        public List<string> KnownElements;
    }

    [Serializable]
    public class Element
    {
        public string Name;

        public int Count;
    }

    [Serializable]
    public class ElementPrototype : Prototype
    {
        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public int SpritePath;

        [XmlElement("Receipe")]
        public List<ElementPrototypeReceipe> ElementReceipes;

        [XmlElement("Stat")]
        public List<ElementPrototypeStat> ElementStats;

        [XmlAttribute]
        public string UnlockTowerTemplate;
    }

    [Serializable]
    public class ElementPrototypeReceipe
    {
        [XmlElement("Ingredient")]
        public List<string> Ingredients;
    }

    [Serializable]
    public class ElementPrototypeStat
    {
        public TowerSlotType InSlot;

        public DamageType ArmorBonus;

        public DamageType DamageBonus;

        public int HpBonus;

        public int TargetBonus;

        public int RangeBonus;
    }

    [Serializable]
    public enum TowerSlotType
    {
        Base,
        Tower,
        Weapon,
        Enchant
    }

    [Serializable]
    public class FactoryTemplate : Prototype
    {
        [XmlAttribute]
        public string Name;

        //auto combine resources
    }

    [Serializable]
    public class TowerTemplate : Prototype
    {
        [XmlAttribute]
        public string Name;

        [XmlElement("Slot")]
        public List<TowerTemplateSlot> Slots;
    }

    [Serializable]
    public class TowerTemplateSlot
    {
        public DamageType ArmorBase;

        public DamageType DamageBase;

        public int HpBase;

        public int TargetBase;

        public int RangeBase;
    }

    [Serializable]
    public class Tower
    {

    }

    [Serializable]
    public class TowerPlot
    {

    }

    [Serializable]
    public class SpawnPoint
    {
        public string Name;

        public float X;
        public float Y;
        public float Z;
    }

    [Serializable]
    public class MonsterPath
    {
        public int Id;
        public Vector3 OringinX;
        public float OringinY;
        public float OringinZ;

        public List<MonsterCheckpoint> MonsterCheckpoints;

        public List<string> DestinationPaths;

        public bool EndInStronghold;
        
        public List<string> BlokerPlots;
    }

    [Serializable]
    public class MonsterCheckpoint
    {
        public int Id;
        public Vector3 OringinX;
        public float OringinY;
        public float OringinZ;
        
    }

    [Serializable]
    public class Monster
    {
    }

    [Serializable]
    public class MonsterPrototype : Prototype
    {
    }

    [Serializable]
    public class MonsterWaves
    {
    }


    [Serializable]
    public enum DamageType
    {
        Blunt,
        Burning,
        //...
    }

    [Serializable]
    public class Level : Prototype
    {
        public string Name;

        public int Order;

        public string ModelName;

        public List<TowerPlot> TowerPlots;

        public List<SpawnPoint> SpawnPoints;

        public List<MonsterPath> MonsterPath;

        public List<MonsterWaves> MonsterWaves;
    }
}