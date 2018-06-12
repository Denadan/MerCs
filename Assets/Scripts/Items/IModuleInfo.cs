namespace Mercs.Items
{
    public interface IModuleInfo
    {
        string Name { get; }
        string ShortName { get; }
        float Weight { get; }
        SlotSize Slot { get; }
        ModuleType ModType { get; }
    }
}
