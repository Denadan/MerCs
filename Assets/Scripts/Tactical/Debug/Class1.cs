using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mercs
{
    [Serializable]
    public class StartMechInfo
    {
        public string Name;
        public Sprite MechSprite;
        public int MovePoints;
        public int RunPoints;
        public int JumpPoints;
    }
}
