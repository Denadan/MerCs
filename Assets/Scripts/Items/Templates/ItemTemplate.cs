using UnityEngine;


namespace Mercs.Items
{
    public abstract class ItemTemplate : ScriptableObject
    {
        public string Name;
        public string ShortName;
        public Sprite Icon;

        public UpgradeTemplate[] Upgrades;
    }
}
