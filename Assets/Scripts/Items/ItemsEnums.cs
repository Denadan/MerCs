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
        Weight = 0,

        Damage = 1,
        StabDamage = 2,
        HeatDamage =3,

        Capacity = 4,
        DamageTransfer =5,
        DamageToHeat = 6,
        DamageToStab = 7,

        Range = 8,
        Accuracy = 9,

        EnergyPerShot = 10,
        HeatPerShot = 11,

        GuidanceSystem = 12,
        GuidanceBonus = 13,
        Shots = 14
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
    
    public enum GuidanceSystem
    {
        None = 0, Termal = 1, Laser = 2, Beacon = 3, Telemetry = 4
    }
}