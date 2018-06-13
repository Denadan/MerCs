using System;
using Mercs.Items;
using Mercs.Tactical.Buffs;
using UnityEngine;

namespace Mercs.Tactical
{
    public static class UnitContructor
    {
        public static UnitInfo Build(GameObject prefab, UnitTemplate template, PilotInfo pilot)
        {
            var info = GameObject.Instantiate(prefab).GetComponent<UnitInfo>();
            if(info == null)
                throw new ArgumentException($"{prefab} is not Unit!");

            info.gameObject.SetActive(true);

            info.GetComponent<SpriteRenderer>().sprite = template.Sprite;
            
            info.Weight = template.Weight;
            info.Reserve = true;
            info.Position.position = new Vector2Int(-1, -1);

            // TODO: НЕ ЗАБЫТЬ ПЕРЕДЕЛАТЬ БЕЗ ИНСТАНИЦИИ
            info.Modules = info.UnitHP.Build(template);
            info.Engine = info.Modules.Find(i => i.ModType == ModuleType.Reactor) as Reactor;
            info.Movement.Build(info);

            AddPilot(info, pilot, template.AddHp);
              
            return info;

        }

        public static bool AddPilot(UnitInfo info, PilotInfo pilot, int add_hp)
        {

            if (pilot == null)
            {
                info.PilotName = "DroneAI";
                GameObject.Destroy(info.PilotHP);
                info.PilotHP = null;
                info.Buffs.Add(new BuffDescriptor
                {
                    Type = BuffType.InitiativeBonus,
                    Value = -1,
                    TAG = "No Pilot",
                    MinVision = Visibility.Scanned,
                    ToolTip = "Initiative {0}"
                });
                return true;
            }
            else
            {
                info.PilotName = pilot.name;
                info.PilotHP.Init(pilot, add_hp);

                if (pilot.InitiativeBonus > 0)
                    info.Buffs.Add(new BuffDescriptor
                    {
                        Type = BuffType.InitiativeBonus,
                        Value = pilot.InitiativeBonus,
                        TAG = "Master Pilot",
                        MinVision = Visibility.Scanned,
                        ToolTip = "Initiative {0}"
                    });
                return false;
            }
        }
    }
}