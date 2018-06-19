using System.Text;
using UnityEngine;

namespace Mercs.Items
{
    /// <summary>
    /// Охладитель
    /// </summary>
    [CreateAssetMenu(fileName = "HeatSink", menuName = "MerCs/Module/HeatSink")]
    public class HeatSink : ModuleInfo<HeatSinkTemplate>, IHeatDissipator, IHeatContainer
    {
        public override ModuleType ModType => ModuleType.HeatSink;
        public override bool Unique => false;

        /// <summary>
        /// рассеивание тепла в ход
        /// </summary>
        public float HeatDissipation { get; private set; }
        /// <summary>
        /// бонусная емкость тепла
        /// </summary>
        public float HeatCapacity { get; private set; }

        /// <summary>
        /// применение апгрейда
        /// ВАЖНО: до использования этого метода инфа модуля не верна!
        /// </summary>
        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();

            HeatCapacity = upgrade(UpgradeType.HeatCapacity, Template.HeatCapacity);
            HeatDissipation = upgrade(UpgradeType.Heat, Template.HeatDissipation);
        }
#if UNITY_EDITOR
        public override string ToString()
        {
            if (Template == null || Upgrade == null)
                return "NO TEMPLATE!";

            StringBuilder sb = new StringBuilder();

            sb.Append("HeatSink\n");
            sb.Append(base.ToString());
            sb.Append($"\nHeat: {HeatCapacity:F2}-{HeatDissipation:F2}\n");

            return sb.ToString();
        }
#endif 
    }
}
