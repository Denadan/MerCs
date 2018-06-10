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
        }

        public string TAG = "";
        public BuffType Type = BuffType.None;
        public float Value = 0;
        public BuffDuration Duration = BuffDuration.Permanent;
        public Visibility MinVision = Visibility.None;
        public bool Stackable = true;

    }
}