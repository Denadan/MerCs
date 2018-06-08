﻿using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Mercs.Tactical
{
    public class UnitHp : MonoBehaviour
    {
        /// <summary>
        /// часть
        /// </summary>
        private class part
        {
            /// <summary>
            /// место части
            /// </summary>
            public Parts place;

            public bool destroyed;
            public bool cripped;

            public bool has_back_armor;
            public bool critical;

            public Vector3 current_hp;
            public Vector3 max_hp;
            public Vector3 total_damage;

            public List<part> childrens;
            public part transfer;

            public UnitPart template;

            public part(UnitPart template)
            {
                has_back_armor = template.HasBackArmor;
                critical = template.Critical;
                destroyed = false;
                cripped = false;
                childrens = new List<part>();
                place = template.Place;

                current_hp = max_hp = new Vector3(
                    template.Structure,
                    template.Armor,
                    template.HasBackArmor ? template.BackArmor : 0);
            }

        }

        private Dictionary<Parts, part> parts;
        List<(Parts place, float w)> front, back, left, right;

        public void Build(UnitTemplate template)
        {
            var list = (from item in template.PartTable select (part: new part(item), template: item)).ToList();
            parts = list.ToDictionary(i => i.template.Place, i => i.part);

            front = template.PartTable.Select(i => (i.Place, (float)i.Size[0])).ToList();
            left = template.PartTable.Select(i => (i.Place, (float)i.Size[1])).ToList();
            right = template.PartTable.Select(i => (i.Place, (float)i.Size[2])).ToList();
            back = template.PartTable.Select(i => (i.Place, (float)i.Size[3])).ToList();

            foreach (var p in list)
            {
                if (p.template.DependOn != Parts.None)
                    parts[p.template.DependOn].childrens.Add(p.part);
                if (p.template.TransferTo != Parts.None)
                    p.part.transfer = parts[p.template.TransferTo];


            }
        }

        public float TotalArmor => parts.Sum(i => i.Value.current_hp.y + i.Value.current_hp.z);
        public float TotalStructure => parts.Sum(i => i.Value.current_hp.x);
        public float MaxArmor => parts.Sum(i => i.Value.max_hp.y + i.Value.max_hp.z);
        public float MaxStructure => parts.Sum(i => i.Value.max_hp.x);
        public Parts[] AllParts => parts.Keys.ToArray();
        public bool PartDestroyed(Parts p) => parts.TryGetValue(p, out var part) && (part.cripped || part.destroyed);
        public (float structure, float armor, float back_armor, bool has_back_armor) CurrentHp(Parts p) =>
            !parts.TryGetValue(p, out var part) ?
            (part.current_hp.x, part.current_hp.y, part.current_hp.z, part.has_back_armor) :
            (0, 0, 0, false);
        public (float structure, float armor, float back_armor, bool has_back_armor) MaxHp(Parts p) =>
            parts.TryGetValue(p, out var part) ? 
            (part.max_hp.x, part.max_hp.y, part.max_hp.z, part.has_back_armor) : 
            (0, 0, 0, false);
    }
}