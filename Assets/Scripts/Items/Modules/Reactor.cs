using UnityEngine;

namespace Mercs.Items
{
    [CreateAssetMenu(fileName = "Reactor", menuName = "MerCs/Module/Reactor")]
    public class Reactor : ModuleInfo<ReactorTemplate>
    {
        public int Size;

        private float weight;

        public override float Weight => weight;

        public override void ApplyUpgrade()
        {
            weight = Template.BaseWeight + Size * (Size * Template.SlotAddWeight + Template.SlotWeight);
            weight = upgrade(UpgradeType.Weight, weight);

            base.ApplyUpgrade();
        }
    }
}