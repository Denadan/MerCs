using UnityEngine;

namespace Mercs.Items
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "MerCs/Template/Weapon")]
    public class WeaponTemplate : ModuleTemplate
    {
        public WeaponType Type;
        public DamageType DamageType;
        public Ammo StockAmmo;

        public AmmoType Ammo;

        public float EnergyForShot;
        public float HeatForShot;
        public bool VariableShots;

        public int Shots;
        public float DamageMult;

        public float MinRange;
        public float Optimal;
        public float Falloff;

        public bool DirectFire = true;
        public bool IndirectFire = false;
        public bool SupportFire = false;

    }
}