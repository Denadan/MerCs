using System;
using System.Collections.Generic;

namespace Mercs.Tactical.Buffs
{
    [Serializable]
    public class BuffDescriptor
    {
        public enum BuffDuration
        {
            Permanent,
            EndOfRound,
            EndOfPhase,
            BeginNextTurn,
            EndNextTurn,
            Projected
        }

        private string tooltip;

        public string TAG = "";
        public BuffType Type = BuffType.None;
        public float Value = 0;
        public BuffDuration Duration = BuffDuration.Permanent;
        public Visibility.Level MinVision = Visibility.Level.None;
        public bool Stackable = true;
        public UnitInfo ProjectedSouce = null;

        public string ToolTip
        {
            set => tooltip = value;
            get => string.Format(tooltip, Value);

        }
    }
}