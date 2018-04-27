using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers.Game.UI
{
    public class ElementListItemController : MonoBehaviour
    {
        public Text Text;

        public Element Element;
        public ElementPrototype ElementPrototype;

        public TooltipProvider TooltipProvider;

        public void Build()
        {
            var partNames = new Dictionary<TowerSlotType, string>
                    {
                        {TowerSlotType.Base, "pedestal"},
                        {TowerSlotType.Body, "body"},
                        {TowerSlotType.Weapon, "weapon"},
                    };

            Text.text = "" + Element.Count;

            string stats = ElementPrototype.Name + Environment.NewLine;

            if (ElementPrototype.ElementStats == null || !ElementPrototype.ElementStats.Any())
            {
                stats += "Not usable for construction";
            }
            else
            {
                stats += Environment.NewLine + "Position (Range, Speed, Damage)" + Environment.NewLine;
                foreach (var elementStat in ElementPrototype.ElementStats)
                {
                    stats += partNames[elementStat.InSlot] + "(" +
                             elementStat.RangeBonus + ", " +
                             elementStat.SpeedBonus + ", " +
                             elementStat.DamageAmount + " " +
                             elementStat.DamageType + ")" + Environment.NewLine;
                }
            }

            TooltipProvider.MultipleContent = new Dictionary<string, string>
            {
                { "default", ElementPrototype.Name },
                { "ElementsStats", stats }
            };
        }
    }
}