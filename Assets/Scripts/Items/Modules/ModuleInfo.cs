
namespace Mercs.Items
{
    public abstract class ModuleInfo<T> : ItemInfo<T>, IModuleInfo
        where T : ModuleTemplate
    {
        private float weight;

        public virtual float Weight => weight;
        public SlotSize Slot => Template.slots;
        public virtual int Crit { get => Template.Crits; }

        public abstract ModuleType ModType { get; }

        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();
            weight = upgrade(UpgradeType.Weight, Template.Weight);
        }
#if UNITY_EDITOR
        public override string ToString()
        {
            var name = base.ToString();
            return name + $"\nWeight: {Weight:F2} Size:{Slot} Crit:{Crit}";
        }
#endif 
    }
}
