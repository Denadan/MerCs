namespace Mercs.Items
{
    public interface IModuleInfo
    {
        string Name { get; }
        string ShortName { get; }
        float Weight { get; }
        SlotSize Slot { get; }
        ModuleType ModType { get; }

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
}
