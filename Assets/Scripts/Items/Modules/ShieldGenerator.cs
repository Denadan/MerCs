﻿using System.Text;
using UnityEngine;

namespace Mercs.Items
{
    [CreateAssetMenu(fileName = "ShieldGenerator", menuName = "MerCs/Module/ShieldGenerator")]
    public class ShieldGenerator : ModuleInfo<ShieldTemplate>, IShield, IShieldRegenerator, IPassiveHeatPoducer
    {
        
        public override ModuleType ModType => ModuleType.Shield;
        public override bool Unique => false;
        /// <summary>
        /// обем щита
        /// </summary>
        public float Shield { get; private set; }
        /// <summary>
        /// восстановление щита
        /// </summary>
        public float ShieldRegen { get; private set; }
        /// <summary>
        /// задржка перезарядки после полного истощения 
        /// </summary>
        public int ShieldDelay => Template.ShieldDelay;
        /// <summary>
        /// Производимое тепло
        /// </summary>
        public float HeatPerRound { get; private set; }

        /// <summary>
        /// применение апгрейда
        /// ВАЖНО: до использования этого метода инфа модуля не верна!
        /// </summary>
        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();

            Shield = upgrade(UpgradeType.Stability, Template.Shield);
            ShieldRegen = upgrade(UpgradeType.StabilityRestore, Template.ShieldRegen);
            HeatPerRound = upgrade(UpgradeType.Heat, Template.Heat);
        }

#if UNITY_EDITOR
        public override string ToString()
        {
            if (Template == null || Upgrade == null)
                return "NO TEMPLATE!";

            StringBuilder sb = new StringBuilder();

            sb.Append($"Shield)\n");
            sb.Append(base.ToString());
            sb.Append($"\nShield: {Shield} + {ShieldRegen}");
            sb.Append($"\nHeat: {HeatPerRound}\n");


            return sb.ToString();
        }
#endif 
    }
}