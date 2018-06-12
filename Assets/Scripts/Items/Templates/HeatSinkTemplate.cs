
using UnityEngine;

namespace Mercs.Items
{
    [CreateAssetMenu(fileName = "HeatSink", menuName = "MerCs/Template/HeatSink")]
    public class HeatSinkTemplate : ModuleTemplate
    {
        public float HeatDissipation;
        public float HeatCapacity;
    }
}
