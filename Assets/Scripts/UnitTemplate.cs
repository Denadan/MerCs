using Mercs.Tactical;
using UnityEngine;
using System;
using Mercs.Items;

#if UNITY_EDITOR
using System.Linq;
using System.Text;
#endif

namespace Mercs
{
    public class UnitTemplate : ScriptableObject
    {
        [Serializable]
        public class Equip
        {
            public Parts place;
            public ScriptableObject Module;
            public Ammo Ammo;

            public IModuleInfo module => Module as IModuleInfo;
        }

        [Range(25, 120)]
        public int Weight;
        public Sprite Sprite;
        public int AddHp;

        public UnitType Type;

        public MercClass Class
        {
            get
            {
                switch (Weight)
                {
                    case int w when w <= 40:
                        return MercClass.Recon;
                    case int w when w <= 60:
                        return MercClass.Light;
                    case int w when w <= 80:
                        return MercClass.Medium;
                    case int w when w <= 100:
                        return MercClass.Heavy;
                    default:
                        return MercClass.Behemot;
                }
            }
        }

        public int Initiative
        {
            get
            {
                switch (Weight)
                {
                    case int w when w <= 40:
                        return 2;
                    case int w when w <= 60:
                        return 2;
                    case int w when w <= 80:
                        return 3;
                    case int w when w <= 100:
                        return 3;
                    default:
                        return 4;
                }
            }
        }

        [Header("Movement")]
        public int MoveSpeed;
        public int RunSpeed;
        public int Jumps;

        [Header("Parts")]
        public float Shield;
        public float ShieldRegen;
        public UnitPart[] PartTable;

        [Header("Items")]
        public Equip[] Items;

#if UNITY_EDITOR
        public void SetType(UnitType type)
        {
            Type = type;
            switch (type)
            {
                case UnitType.Turret:
                    PartTable = new UnitPart[] {
                        new UnitPart { Place = Parts.TT, Critical = true }
                    };
                    break;
                case UnitType.Tank:
                    PartTable = new UnitPart[] {
                        new UnitPart {  Place = Parts.TC, Critical = true },
                        new UnitPart {  Place = Parts.RC, TransferTo = Parts.TC},
                        new UnitPart {  Place = Parts.LC, TransferTo = Parts.TC},
                        new UnitPart {  Place = Parts.FC, TransferTo = Parts.TC},
                        new UnitPart {  Place = Parts.BC, TransferTo = Parts.TC},

                    };
                    break;
                case UnitType.Vehicle:
                    PartTable = new UnitPart[] {
                        new UnitPart {  Place = Parts.VC, Critical = true },
                        new UnitPart {  Place = Parts.RC, TransferTo = Parts.VC},
                        new UnitPart {  Place = Parts.LC, TransferTo = Parts.VC},
                        new UnitPart {  Place = Parts.FC, TransferTo = Parts.VC},
                        new UnitPart {  Place = Parts.BC, TransferTo = Parts.VC},
                        new UnitPart {  Place = Parts.LM, TransferTo = Parts.VC},
                    };
                    break;
                case UnitType.MerC:
                    PartTable = new UnitPart[] {
                        new UnitPart {  Place = Parts.HD, Critical = true },
                        new UnitPart {  Place = Parts.CT, Critical = true, HasBackArmor = true},
                        new UnitPart {  Place = Parts.LT, TransferTo = Parts.CT, HasBackArmor = true},
                        new UnitPart {  Place = Parts.RT, TransferTo = Parts.CT, HasBackArmor = true},
                        new UnitPart {  Place = Parts.LH, TransferTo = Parts.LT, DependOn = Parts.LT},
                        new UnitPart {  Place = Parts.RH, TransferTo = Parts.RT, DependOn = Parts.RT},
                        new UnitPart {  Place = Parts.LL, TransferTo = Parts.LT},
                        new UnitPart {  Place = Parts.RL, TransferTo = Parts.RT},
                    };
                    break;
            }
            foreach (var part in PartTable)
                part.SetDefaultSize();
        }
        public bool NeedUpdate
        {
            get => true;
        }
        public void Update()
        {
            if (PartTable == null || PartTable.Length == 0)
                SetType(Type);
            foreach (var part in PartTable.Where(i => i.Size.All(p => p == 0)))
                part.SetDefaultSize();
            if (Type == UnitType.MerC)
            {
                MoveSpeed = 0;
                Jumps = 0;
                RunSpeed = 0;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString() + "\n");

            if (Items != null)
            {
                foreach (var item in Items.Where(i => i.module != null))
                {
                    item.module.ApplyUpgrade();
                    if (item.Ammo != null)
                        item.Ammo.ApplyUpgrade();
                }
                    var w = Items.Where(i => i.module != null).Sum(i => i.module.Weight);
                sb.Append($"Fit Weight:{w:F2}\n");

                //w = 0;
                //foreach(var item in Items.Where(i => i.module != null))
                //{
                //    sb.Append($"{item.module.ShortName}:{item.module.Weight:F2}\n");
                //}
                //sb.Append($"Fit Weight:{w:F2}\n");

                var engine = Items
                    .Where(i => i.module != null && i.module.ModType == ModuleType.Reactor)
                    .Select(i => i.module as Reactor)
                    .FirstOrDefault();

                if (engine != null)
                {
                    sb.Append($"Move: {engine.EngineRating / Weight}\n");
                    sb.Append($"Run: {engine.EngineRating * 3 / Weight / 2}\n");

                    var jump = Items
                        .Where(i => i.module != null && i.module.ModType == ModuleType.JumpJet)
                        .Select(i => i.module as JumpJet)
                        .Select(i => (er: i.EngineRating, h: i.Heat))
                        .Aggregate(
                            (er: 0, h: 0f),
                            (total, next) => (total.Item1 + next.er, total.Item2 + next.h)
                        );

                    if (jump.er > 0)
                        sb.Append($"Jump: {jump.er / Weight}\n");

                    var heatc = Items
                        .Where(i => i.Module is IHeatContainer)
                        .Select(i => (i.module as IHeatContainer).HeatCapacity)
                        .Sum();

                    var heatp = Items
                        .Where(i => i.Module is IHeatProducer)
                        .Select(i => (i.module as IHeatProducer).Heat)
                        .Sum();

                    var heatd = Items
                        .Where(i => i.Module is IHeatDissipator)
                        .Select(i => (i.module as IHeatDissipator).HeatDissipation)
                        .Sum();

                    sb.Append($"Heat: {heatc:F2} +{heatp:F2}(J:{jump.h:F2}) -{heatd:F2}\n");

                    var ammo_list = Items
                        .Where(i => i.module != null && i.module.ModType == ModuleType.AmmoPod && i.Ammo != null)
                        .Select(i => (pod: i.Module as AmmoPod, ammo: i.Ammo))
                        .Where(i => i.pod.Type == i.ammo.Type)
                        .GroupBy(
                            i => i.ammo,
                            i => i.pod.Capacity,
                            (key, item) => new { ammo = key, sum = item.Sum() })
                        .GroupBy(i => i.ammo.Type);
                    sb.Append("============== AMMO =============\n");
                    foreach(var item in ammo_list)
                    {
                        sb.Append($"{item.Key}:\n");
                        foreach (var i in item)
                            sb.Append($"   {i.ammo.Name}: {i.sum}\n");
                    }
                    sb.Append("================================\n");

                }
                else
                    sb.Append("No Engine!");


            }
            else
            {
                sb.Append("EMPTY!");
            }

            return sb.ToString();
        }
#endif
    }
}
