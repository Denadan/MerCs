using UnityEngine;

namespace Mercs
{
    public class WeaponTemplate : ScriptableObject
    {
        public WeaponType Type;
        public DamageType DamageType;

        public string descriptin;
        public Sprite Icon;

        public float Weight;
        public SlotSize slots;

        public float EnergyForShot;
        public float HeatForShot;
        public bool VariableShots;

        public int Shots;
        public float Damage;
        public float Heat;
        public float Stability;

        public float MinRange;
        public float Optimal;
        public float Falloff;

        public bool DirectFire = true;
        public bool IndirectFire = false;

        public AmmoType Ammo;

        public WeaponUpgrade[] Upgrades;
    }
}