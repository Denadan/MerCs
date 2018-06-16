#pragma warning disable 649

using System.Collections.Generic;
using Mercs.Items;
using UnityEngine;

namespace Mercs.Tactical.UI
{
    public class WeaponList : MonoBehaviour
    {
        [SerializeField] private Transform Container;
        [SerializeField] private GameObject ItemPrefab;

        private Dictionary<Weapon, WeaponInfoItem> items = new Dictionary<Weapon, WeaponInfoItem>();

        public void Set(UnitInfo info)
        {
            items.Clear();

            foreach (Transform child in Container)
                Destroy(child.gameObject);

            if(info == null)
                return;

            foreach (var infoWeapon in info.Weapons)
            {
                var item = Instantiate(ItemPrefab, Container, false).GetComponent<WeaponInfoItem>();
                item.Set(infoWeapon);
                items.Add(infoWeapon.Weapon, item);
            }
        }
    }
}
