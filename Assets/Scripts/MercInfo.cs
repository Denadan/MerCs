using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mercs
{
    public class MercInfo : ScriptableObject
    {

        [Range(25,120)]
        public int Weight;
        public Sprite Sprite;
        public int AddHp;

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

    }
}
