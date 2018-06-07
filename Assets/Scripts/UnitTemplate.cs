using Mercs.Tactical;
using UnityEngine;

namespace Mercs
{
    public class UnitTemplate : ScriptableObject
    {
        [Range(25,120)]
        public int Weight;
        public Sprite Sprite;
        public int AddHp;

        public UnitType Type;

        public MercClass Class
        {
            get
            {
                switch(Weight)
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
        public UnitPart[] PartTable;

        public void SetType(UnitType type)
        {
            Type = type;
            switch(type)
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
        }
    }
}
