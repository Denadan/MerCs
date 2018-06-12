using UnityEngine;

namespace Mercs.Items
{
    [CreateAssetMenu(fileName = "Reactor", menuName = "MerCs/Template/Reactor")]
    public class ReactorTemplate : ModuleTemplate
    {
        [Header("Base Data")]
        public int BaseSize;
        public int BaseEngineRating;
        public float BaseHeatSink;
        public float BaseHeatCapacity;
        public float BaseWeight;
        public int BaseExtend;
        public int BaseCrit;
        
        [Header("Slot Data")]
        public int SlotEngineRating;
        public float SlotSubEr;

        public float SlotHeatSink;
        public float SlotHeatCapacity;
        public float SlotExtend;
        public int ExtendSlotCrit;
        public float CentralSlotCrit;

        public float SlotWeight;
        public float SlotAddWeight;
    }
}