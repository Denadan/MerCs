
namespace Mercs.Items
{
    public class ModuleInfo<T> : ItemInfo<T>
        where T : ModuleTemplate
    {

        public float Weight { get; private set; }
        public SlotSize Slot => Template.slots;

        public override void ApplyUpgrade()
        {
            base.ApplyUpgrade();
            Weight = upgrade(UpgradeType.Weight, Template.Weight);
        }
#if UNITY_EDITOR
        public override string ToString()
        {
            var name = base.ToString();
            return name + $"\nWeight: {Weight:F2} Size:{Slot}";
        }
#endif 
    }
}
