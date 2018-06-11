namespace Mercs.Items
{
    public enum WeaponType
    {
        Laser,
        GuidedMissile,
        DirectFireMissile,
        Artillery,
        Autocannon,
        IonCannon
    }

    public enum UpgradeType
    {
        Weight,

        Damage,
        StabDamage,
        HeatDamage,

        Capacity,
        DamageTransfer,
        DamageToHeat,
        DamageToStab,

        Range,
        Accuracy,

        EnergyPerShot,
        HeatPerShot
    }

    public enum DamageType
    {
        Missile,
        Ballistic,
        Energy
    }

    public enum SlotSize
    {
        One,
        Two,
        Three,
        ThreeLine,
        Four
    }

    public enum AmmoType
    {
        None,
        Missile,
        Rocket,
        ArtS,
        ArtM,
        ArtL,
        ArtXL,
        ACS,
        ACM,
        ACL,
        ACXL,
        MG
    }

    public enum Rarity
    {
        Stock,
        Normal,
        Improved,
        Advanced
    }
}