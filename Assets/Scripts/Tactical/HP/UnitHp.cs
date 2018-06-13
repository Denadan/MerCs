﻿using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Runtime.ConstrainedExecution;
using Mercs.Items;
using EventHandler = Mercs.Tactical.Events.EventHandler;

namespace Mercs.Tactical
{
    public class UnitHp : MonoBehaviour
    {

        private class crit
        {
            public int value;
            public IModuleInfo module;

            public crit(int val, IModuleInfo mod)
            {
                value = val;
                module = mod;
            }

        }
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

            public List<crit> crit_table;

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

                crit_table = new List<crit>();
                //current_hp.x *= UnityEngine.Random.Range(0f, 1f);
                //current_hp.y *= UnityEngine.Random.Range(0f, 1f);
                //current_hp.z *= UnityEngine.Random.Range(0f, 1f);
            }
            #region events
            private List<GameObject> subscribers = new List<GameObject>();

            public void Subscrive(GameObject go)
            {
                subscribers.Add(go);
            }

            public void UnSubscribe(GameObject go)
            {
                subscribers.Remove(go);
            }
            #endregion
        }

        

        private Dictionary<Parts, part> parts;
        private List<GameObject> subscribers = new List<GameObject>();
        private List<IModuleInfo> modules;

        List<(Parts place, float w)> front, back, left, right;

        public float MaxShield { get; private set; }
        public float Shield { get; private set; }
        public float ShieldRegen { get; set; }

        public IEnumerable<IModuleInfo> Modules()
        {
            return modules;
        }

        public IEnumerable<IModuleInfo> Modules(Parts part)
        {
            if (parts.TryGetValue(part, out var p))
                return p.crit_table.Select(i => i.module);

            return null;
        }

        private void Start()
        {
            EventHandler.RegisterUnitHp(GetComponent<UnitInfo>(), this);
        }

        private void OnDestroy()
        {
            EventHandler.UnRegisterUnitHp(GetComponent<UnitInfo>());
        }

        public List<IModuleInfo> Build(UnitTemplate template)
        {
            modules = new List<IModuleInfo>();

            var list = (from item in template.PartTable select (part: new part(item), template: item)).ToList();
            parts = list.ToDictionary(i => i.template.Place, i => i.part);

            front = template.PartTable.Select(i => (i.Place, (float)i.Size[0])).ToList();
            left = template.PartTable.Select(i => (i.Place, (float)i.Size[1])).ToList();
            right = template.PartTable.Select(i => (i.Place, (float)i.Size[2])).ToList();
            back = template.PartTable.Select(i => (i.Place, (float)i.Size[3])).ToList();

            foreach (var item_info in template.Items)
            {
                //UnityEngine.Debug.Log($"{template.name}  {item_info.place}\n{item_info.Module}");
                var module = Instantiate(item_info.Module) as IModuleInfo;
                module.ApplyUpgrade();
                modules.Add(module);

                if (module.ModType == ModuleType.Reactor)
                {
                    var reactor = module as Reactor;
                    if (reactor.SideSlot > 0)
                    {
                        parts[Parts.LT].crit_table.Add(new crit(reactor.SideSlot, module));
                        parts[Parts.RT].crit_table.Add(new crit(reactor.SideSlot, module));
                    }
                    parts[item_info.place].crit_table.Add(new crit(reactor.CentralSlot, module));
                }
                else
                {
                    int size = 0;
                    switch (item_info.module.Slot)
                    {
                        case SlotSize.IOne:
                        case SlotSize.One:
                            size = 1;
                            break;
                        case SlotSize.ITwo:
                        case SlotSize.Two:
                            size = 2;
                            break;
                        case SlotSize.IThree:
                        case SlotSize.Three:
                        case SlotSize.IThreeLine:
                        case SlotSize.ThreeLine:
                            size = 3;
                            break;
                        case SlotSize.Four:
                        case SlotSize.IFour:
                            size = 4;
                            break;
                        case SlotSize.IFive:
                        case SlotSize.Five:
                            size = 5;
                            break;
                    }
                    if(size > 0)
                        parts[item_info.place].crit_table.Add(new crit(size, module));
                }
            }

            MaxShield = modules.OfType<IShield>().Sum(i => i.Shield);
            Shield = MaxShield;
            ShieldRegen = modules.OfType<IShieldRegenerator>().Sum(i => i.ShieldRegen);
            
            foreach (var p in list)
            {
                if (p.template.DependOn != Parts.None)
                    parts[p.template.DependOn].childrens.Add(p.part);
                if (p.template.TransferTo != Parts.None)
                    p.part.transfer = parts[p.template.TransferTo];
            }

            return modules;
        }

        public void Regen(float value)
        {
            Shield = Mathf.Clamp(Shield + value, 0, MaxShield);
        }

        public float TotalArmor => parts.Sum(i => i.Value.current_hp.y + i.Value.current_hp.z);
        public float TotalStructure => parts.Sum(i => i.Value.current_hp.x);
        public float MaxArmor => parts.Sum(i => i.Value.max_hp.y + i.Value.max_hp.z);
        public float MaxStructure => parts.Sum(i => i.Value.max_hp.x);
        public Parts[] AllParts => parts.Keys.ToArray();
        public bool PartDestroyed(Parts p) => parts.TryGetValue(p, out var part) && (part.cripped || part.destroyed);
        public (float structure, float armor, float back_armor, bool has_back_armor) CurrentHp(Parts p) =>
            parts.TryGetValue(p, out var part) ?
            (part.current_hp.x, part.current_hp.y, part.current_hp.z, part.has_back_armor) :
            (0, 0, 0, false);
        public (float structure, float armor, float back_armor, bool has_back_armor) MaxHp(Parts p) =>
            parts.TryGetValue(p, out var part) ?
            (part.max_hp.x, part.max_hp.y, part.max_hp.z, part.has_back_armor) :
            (0, 0, 0, false);

        public void Subscribe(GameObject go, Parts part)
        {
            if (parts.TryGetValue(part, out var p))
                p.Subscrive(go);
        }

        public void UnSubscribe(GameObject go, Parts part)
        {
            if (parts.TryGetValue(part, out var p))
                p.UnSubscribe(go);
        }

        internal void Subscribe(GameObject go)
        {
            if (go != null)
                subscribers.Add(go);
        }

        internal void UnSubscribe(GameObject go)
        {
            subscribers.Remove(go);
        }
    }
}
