using System;
using System.Linq;
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

            info.Type = template.Type;

            info.GFX.SetData(template);
            
            info.Weight = template.Weight;
            info.Reserve = true;
            info.Position.position = new Vector2Int(-1, -1);

            info.Modules.Build(template);
            info.UnitHP.Build(template, info.Modules);
            info.Weapons.Build();

            info.RadarRange = template.RadarRange;
            info.VisualRange = template.VisualRange;
            info.ScanRange = template.ScannerRange;

            info.Movement.Build();

            AddPilot(info, pilot, template.AddHp);
              
            return info;

        }

        public static bool AddPilot(UnitInfo info, PilotInfo pilot, int add_hp)
        {

            if (pilot == null)
            {
                info.VisualRange = 0;
                info.PilotName = "DroneAI";
                GameObject.Destroy(info.PilotHP);
                info.PilotHP = null;
                info.Buffs.Add(new BuffDescriptor
                {
                    Type = BuffType.InitiativeBonus,
                    Value = -1,
                    TAG = "No Pilot",
                    MinVision = Visibility.Level.Scanned,
                    ToolTip = "Initiative {0}"
                });
                info.Movement.CanDoEvasive = false;
                return true;
            }
            else
            {
                info.PilotName = pilot.name;
                info.PilotHP.Init(pilot, add_hp);
                info.VisualRange += pilot.VisualBonus;

                if (pilot.InitiativeBonus > 0)
                    info.Buffs.Add(new BuffDescriptor
                    {
                        Type = BuffType.InitiativeBonus,
                        Value = pilot.InitiativeBonus,
                        TAG = "Master Pilot",
                        MinVision = Visibility.Level.Scanned,
                        ToolTip = "Initiative {0}"
                    });

                info.Movement.CanDoEvasive = pilot.Evasive;
                return false;
            }
        }
    }
}