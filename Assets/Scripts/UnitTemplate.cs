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

        [Range(25, 125)]
        public int Weight;
        public Sprite Sprite;
        public int AddHp;

        public float RadarRange;
        public float VisualRange;
        public float ScannerRange;

        public UnitType Type;
        public UnitPart[] PartTable;
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
            get => false;
        }
        public void Update()
        {
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
                var items = Items.Where(i => i.module != null).Select(i => i.module).ToList();

                sb.Append($"Class: {CONST.Class(Weight).ToString()}({Weight}) Fit Weight:{w:F2}\n");

                var gyro = items.OfType<Gyro>().FirstOrDefault();
                if (gyro == null)
                    sb.Append("Gyro not installed\n");
                else if (gyro.Class != CONST.Class(Weight))
                    sb.Append("Wrong Gyro!\n");


                var engine = items.OfType<Reactor>().FirstOrDefault();

                if (engine != null)
                {
                    var mmod = items.OfType<IMoveMod>().Aggregate(1f, (t, c) => t * c.MoveMod);
                    var rmod = items.OfType<IRunMod>().Aggregate(1f, (t, c) => t * c.RunMod);
                    var jmod = items.OfType<IJumpMod>().Aggregate(1f, (t, c) => t * c.JumpMod);

                    sb.Append($"Move: {(int)(engine.EngineRating * mmod / Weight) }\n");
                    sb.Append($"Run: {(int)(engine.EngineRating * 1.5f * rmod / Weight)}\n");

                    var jump = items
                        .OfType<JumpJet>()
                        .Select(i => (er: i.EngineRating, h: i.Heat))
                        .Aggregate(
                            (er: 0, h: 0f),
                            (total, next) => (total.Item1 + next.er, total.Item2 + next.h)
                        );

                    var jump_leg = Items
                        .Where(i => i.place == Parts.LL || i.place == Parts.RL)
                        .Select(i => i.module)
                        .OfType<JumpJet>()
                        .Sum(i => i.EngineRating);

                    if (jump.er > 0)
                        sb.Append($"Jump: {(int)(jump.er *jmod / Weight)}({(int)(jump_leg * jmod / Weight)})\n");

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
