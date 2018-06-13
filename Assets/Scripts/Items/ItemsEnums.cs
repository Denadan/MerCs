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
        HeatDamage = 3,

        Capacity = 4,
        DamageTransfer = 5,
        DamageToHeat = 6,
        DamageToStab = 7,

        Range = 8,
        Accuracy = 9,

        RESERVED0 = 10, //!
        Heat = 11,

        GuidanceSystem = 12,
        GuidanceBonus = 13,
        Shots = 14,

        EngineRating = 15,
        HeatCapacity = 16,
        Stability = 17,
        StabilityRestore = 18,
        Shield = 19,
        ShieldRegen = 20,
        RunMod = 21,
        MoveMod = 22,
        JumpMod = 23
    }

    public enum DamageType
    {
        Missile,
        Ballistic,
        Energy
    }

    public enum SlotSize
    {
        One = 0,
        Two = 1,
        Three = 2,
        ThreeLine = 3,
        Four = 4,
        Five = 5,

        Reactor = 6,
        IOne = 7,
        ITwo = 8,
        IThree = 9,
        IThreeLine = 10,
        IFour = 11,
        IFive = 12,
        Gyro = 20
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

    public enum ModuleType
    {
        Reactor = 0,
        Weapon = 1,
        AmmoPod = 2,
        HeatSink = 3,
        JumpJet = 4,
        Gyro = 5,
        Shield = 6,
    }
}