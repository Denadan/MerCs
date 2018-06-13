using System.Collections;
using System.Collections.Generic;
using Mercs.Items;
using UnityEngine;
using System.Linq;

namespace Mercs.Tactical
{
    [RequireComponent(typeof(UnitInfo))]
    public class ModulesData : MonoBehaviour, IEnumerable<IModuleInfo>
    {
        private UnitInfo info;
        private List<IModuleInfo> modules;
        private Dictionary<Parts, List<IModuleInfo>> dictionary;

        public Reactor Reactor { get; private set; }

        public IEnumerable<IModuleInfo> this[Parts part]
            => dictionary.TryGetValue(part, out var list) ? list : Enumerable.Empty<IModuleInfo>();

        private void Awake()
        {
            info = GetComponent<UnitInfo>();
        }

        private IModuleInfo get_module(UnitTemplate.Equip item)
        {
            // TODO: НЕ ЗАБЫТЬ ПЕРЕДЕЛАТЬ БЕЗ ИНСТАНИЦИИ
            var res = ScriptableObject.Instantiate(item.Module) as IModuleInfo;
            res.ApplyUpgrade();

            if (res is AmmoPod pod)
            {
                if (item.Ammo == null || item.Ammo.Type != pod.Type)
                {
                    pod.LoadedAmmo = null;
                    pod.Count = 0;
                }
                else
                {
                    pod.LoadedAmmo = item.Ammo;
                    pod.Count = pod.Capacity;
                }
            }
            else if (res is Weapon weapon)
            {
                if (item.Ammo != null && item.Ammo.Type == weapon.AmmoType)
                {
                    weapon.LoadAmmo(item.Ammo);
                }
            }

            return res;
        }

        public void Build(UnitTemplate template)
        { 
            var list = (from item in template.Items
                where item.module != null
                select (part: item.place, module: get_module(item))).ToList();
            modules = list.Select(i => i.module).ToList();
            dictionary = list.GroupBy(i => i.part)
                .ToDictionary(i=>i.Key, i=>i.Select(a => a.module).ToList());

            Reactor = modules.Find(i => i is Reactor) as Reactor;
        }

        public IEnumerator<IModuleInfo> GetEnumerator()
        {
            return modules.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}