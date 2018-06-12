using System;
using System.Text;
using UnityEngine;

namespace Mercs.Items
{

    [CreateAssetMenu(fileName = "Gyro", menuName = "MerCs/Module/Gyro")]
    public class Gyro : ModuleInfo<GyroTemplate>, IStability, IStabilityRestore, IMoveMod, IRunMod
    {
        [NonSerialized]
        private float weight;


        public MercClass Class;
        public override ModuleType ModType => ModuleType.Gyro;
        public float Stability { get; private set; }
        public float StabilityRestore { get; private set; }
        public float MoveMod { get; private set; }
        public float RunMod { get; private set; }
        public override float Weight => weight;

        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();

            Stability = upgrade(UpgradeType.Stability, Template.Stability + (int) Class * Template.StabilityAdd);
            StabilityRestore = upgrade(UpgradeType.StabilityRestore, Template.StabilityRestore);
            MoveMod = upgrade(UpgradeType.MoveMod, Template.MoveMod);
            RunMod = upgrade(UpgradeType.RunMod, Template.RunMod);
      

            weight = upgrade(UpgradeType.Weight, (int) Class + 3);
            Name = Name.Replace("%CLASS%", Class.ToString());
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


            return sb.ToString();
        }
#endif 
    }
}