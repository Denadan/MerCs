using System.Text;
using UnityEngine;

namespace Mercs.Items
{
    /// <summary>
    /// прыжковый двигатель
    /// </summary>
    [CreateAssetMenu(fileName = "JumpJet", menuName = "MerCs/Module/JumpJet")]
    public class JumpJet : ModuleInfo<JumpJetTemplate>, IHeatProducer, IHeatContainer
    {
        public override ModuleType ModType => ModuleType.JumpJet;
        public override bool Unique => false;

        /// <summary>
        /// нагрев при прижке
        /// </summary>
        public float Heat { get; private set; }
        /// <summary>
        /// мощность двигателя
        /// </summary>
        public int EngineRating { get; private set; }
        /// <summary>
        /// штраф к вместимости нагрева(отрицательное значение)
        /// </summary>
        public float HeatCapacity { get; private set; }

        /// <summary>
        /// применение апгрейда
        /// ВАЖНО: до использования этого метода инфа модуля не верна!
        /// </summary>
        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();

            Heat = upgrade(UpgradeType.Heat, Template.Heat);
            EngineRating = (int)upgrade(UpgradeType.EngineRating, Template.EngineRating);
            HeatCapacity = upgrade(UpgradeType.HeatCapacity, Template.HeatCapacity);
        }

#if UNITY_EDITOR
        public override string ToString()
        {
            if (Template == null || Upgrade == null)
                return "NO TEMPLATE!";

            StringBuilder sb = new StringBuilder();

            sb.Append("JumpDrive\n");
            sb.Append(base.ToString());
            sb.Append($"\nER: {EngineRating} Heat: {Heat:F2} HeatCapacity: {HeatCapacity:F2}\n");

            return sb.ToString();
        }
#endif    
    }
}
