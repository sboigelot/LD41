using System;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class TowerTemplateSlot
    {
        public DamageType ArmorBase;

        public DamageType DamageBase;

        public int HpBase;

        public int TargetBase;

        public int RangeBase;
    }
}