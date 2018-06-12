using System;
using System.Net.Configuration;
using Mercs.Items;
using UnityEditor;
using UnityEngine;

namespace Mercs.Tactical
{
    public static class MechContructor
    {
        public static UnitInfo Build(GameObject prefab, UnitTemplate template)
        {
            var info = GameObject.Instantiate(prefab).GetComponent<UnitInfo>();
            if(info == null)
                throw new ArgumentException($"{prefab} is not Unit!");

            info.GetComponent<SpriteRenderer>().sprite = template.Sprite;

            //info.Active = false;
            //if (item.Pilot == null)
            //{
            //    info.PilotName = "DroneAI";
            //    Destroy(info.PilotHP);
            //    info.PilotHP = null;
            //}
            //else
            //{
            //    info.PilotName = item.Pilot.name;
            //    info.PilotHP.Init(item);
            //}


            
            info.Weight = template.Weight;
            info.Reserve = true;
            info.Position.position = new Vector2Int(-1, -1);

            // TODO: НЕ ЗАБЫТЬ ПЕРЕДЕЛАТЬ БЕЗ ИНСТАНИЦИИ
            info.Modules = info.UnitHP.Build(template);
            info.Engine = info.Modules.Find(i => i.ModType == ModuleType.Reactor) as Reactor;
            info.Movement.Build(info);

            //if (item.Pilot == null)
            //    info.Buffs.Add(new BuffDescriptor
            //    {
            //        Type = BuffType.InitiativeBonus,
            //        Value = -1,
            //        TAG = "No Pilot",
            //        MinVision = Visibility.Scanned,
            //        ToolTip = "Initiative {0}"
            //    });
            //else if (item.Pilot.InitiativeBonus > 0)
            //    info.Buffs.Add(new BuffDescriptor
            //    {
            //        Type = BuffType.InitiativeBonus,
            //        Value = item.Pilot.InitiativeBonus,
            //        TAG = "Master Pilot",
            //        MinVision = Visibility.Scanned,
            //        ToolTip = "Initiative {0}"
            //    });


            return info;
        }

    }
}