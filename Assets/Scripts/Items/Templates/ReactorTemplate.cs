using UnityEngine;

namespace Mercs.Items
{
    [CreateAssetMenu(fileName = "Reactor", menuName = "MerCs/Template/Reactor")]
    public class ReactorTemplate : ModuleTemplate
    {
        public int BaseSize;
        public int BaseEngineRating;
        public float BaseHeatSink;
        public float BaseCapacitator;
        public float BaseRegen;
        public float BaseWeight;
        public int BaseExtend;

        public int SlotEngineRating;
        public float SlotHeatSink;
        public float SlotCapacitator;
        public float SlotRegen;

        public float SlotWeight;
        public float SlotAddWeight;
        public float SlotSubEr;

        public float SlotExtend;
    }
}