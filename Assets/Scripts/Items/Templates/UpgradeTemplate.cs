using System;

namespace Mercs.Items
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