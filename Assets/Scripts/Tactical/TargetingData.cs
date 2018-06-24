using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Mercs.Tactical
{
    public class TargetingInfo
    { 
        public class TargetInfo
        {
            public UnitInfo Target;
            public float Distance;
            public Visibility.Level Level;
            public Visibility.Line Line;
        }

        private UnitInfo Source;
        public int Revision { get; private set; }
        private List<TargetInfo> targets;

        public TargetingInfo(UnitInfo source)
        {
            Revision = 0;
            Source = source;
            targets = new List<TargetInfo>();
        }
    }
}