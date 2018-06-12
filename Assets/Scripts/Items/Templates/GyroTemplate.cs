using UnityEngine;

namespace Mercs.Items
{
    [CreateAssetMenu(fileName = "Gyro", menuName = "MerCs/Template/Gyro")]
    public class GyroTemplate : ModuleTemplate
    {
        public float Stability;
        public float StabilityAdd;
        [Range(0.1f, 0.5f)] public float StabilityRestore;
        [Range(0.5f, 1.5f)] public float MoveMod = 1;
        [Range(0.5f, 1.5f)] public float RunMod = 1;
    }
}