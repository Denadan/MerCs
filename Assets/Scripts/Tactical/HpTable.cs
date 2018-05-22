using System.Collections.Generic;
using UnityEngine;

namespace Mercs.Tactical
{
    public abstract class HpTable : MonoBehaviour
    {
        public abstract void Hit(Dir HitDirection);
        public abstract int PartsCount { get; }
        public abstract (int,int) this[int partNum] { get; }
        public abstract (int,int) Total { get; }
    }
}