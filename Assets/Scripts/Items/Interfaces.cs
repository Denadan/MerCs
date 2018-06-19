using UnityEngine;

namespace Mercs.Items
{
    public interface IModuleInfo
    {
        string Name { get; }
        string ShortName { get; }
        string BaseName { get; }
        float Weight { get; }
        SlotSize Slot { get; }
        ModuleType ModType { get; }
        Sprite Icon { get; }
        bool Unique { get; }

        void ApplyUpgrade();
    }

    public interface IHeatContainer
    {
        float HeatCapacity { get; }
    }

    public interface IHeatDissipator
    {
        float HeatDissipation { get; }
    }

    public interface IHeatProducer
    {
        float Heat { get; }
    }

    public interface IPassiveHeatPoducer
    {
        float HeatPerRound { get; }
    }

    public interface IShield
    {
        float Shield { get; }
    }

    public interface IShieldRegenerator
    {
        float ShieldRegen { get; }
    }

    public interface IStability
    {
        float Stability { get; }
    }

    public interface IStabilityRestore
    {
        float StabilityRestore { get; }
    }

    public interface IRunMod
    {
        float RunMod { get; }
    }
    public interface IMoveMod
    {
        float MoveMod { get; }
    }
    public interface IJumpMod
    {
        float JumpMod { get; }
    }
}
