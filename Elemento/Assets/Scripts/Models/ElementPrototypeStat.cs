using System;

namespace Assets.Scripts.Models
{
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
}