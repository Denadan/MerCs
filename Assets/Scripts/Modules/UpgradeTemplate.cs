using System;

namespace Mercs
{
    [Serializable]
    public class UpgradeTemplate
    {
        public UpgradeType Type;

        public float AddPercent;
        public float SubPercent;

        public float AddValue;
        public float SubValue;

        public int Max;
        public int Min;
    }
}