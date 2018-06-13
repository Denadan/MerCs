using System;
using System.Text;
using UnityEngine;

namespace Mercs.Items
{
    /// <summary>
    /// Гироскоп
    /// </summary>
    [CreateAssetMenu(fileName = "Gyro", menuName = "MerCs/Module/Gyro")]
    public class Gyro : ModuleInfo<GyroTemplate>, IStability, IStabilityRestore, IMoveMod, IRunMod
    {
        [NonSerialized]
        private float weight;

        /// <summary>
        /// Класс меха для которого гироскоп предназначен
        /// </summary>
        public MercClass Class;
        public override ModuleType ModType => ModuleType.Gyro;
        /// <summary>
        /// базовая стабильность
        /// </summary>
        public float Stability { get; private set; }
        /// <summary>
        /// восстановление стабильности при движении
        /// бег = /2
        /// стоять = *2
        /// </summary>
        public float StabilityRestore { get; private set; }
        /// <summary>
        /// модификациия скорости ходьбы
        /// </summary>
        public float MoveMod { get; private set; }
        /// <summary>
        /// модификация скорости бега
        /// </summary>
        public float RunMod { get; private set; }
        /// <summary>
        /// Вес зависит от класса
        /// </summary>
        public override float Weight => weight;

        public int CritSize => (int) (Template.BaseCrit + Template.SizeCrit * (int) Class);

        /// <summary>
        /// применение апгрейда
        /// ВАЖНО: до использования этого метода инфа модуля не верна!
        /// </summary>
        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();

            Stability = upgrade(UpgradeType.Stability, Template.Stability + (int) Class * Template.StabilityAdd);
            StabilityRestore = upgrade(UpgradeType.StabilityRestore, Template.StabilityRestore);
            MoveMod = upgrade(UpgradeType.MoveMod, Template.MoveMod);
            RunMod = upgrade(UpgradeType.RunMod, Template.RunMod);
      

            weight = upgrade(UpgradeType.Weight, (int) Class + 3);
            Name = Name.Replace("%CLASS%", Class.ToString());
            BaseName = BaseName.Replace("%CLASS%", Class.ToString());
        }

#if UNITY_EDITOR
        public override string ToString()
        {
            if (Template == null || Upgrade == null)
                return "NO TEMPLATE!";

            StringBuilder sb = new StringBuilder();

            sb.Append($"Gyro({Class})\n");
            sb.Append(base.ToString());
            sb.Append($"\nStability: {Stability:F2} + {StabilityRestore:P}");
            sb.Append($"\nMove: {MoveMod:P}, Run: {RunMod:P}\n");
            sb.Append($"\nCrit: {CritSize}");


            return sb.ToString();
        }
#endif 
    }
}