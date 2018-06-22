using System.Linq;
using UnityEngine;
using Mercs.Items;

namespace Mercs.Tactical
{
    public class HeatData : MonoBehaviour
    {
        public float Value { get; private set; }
        public float Max { get; private set; }
        public float Threshold { get; private set; }

        public float ThresholdValue => Max * Threshold;
        public bool CanEngineOn => Value - Dissipation + PassiveHeat < Max * Threshold;


        public float Dissipation { get; private set; }
        public float PassiveHeat { get; private set; }



        private UnitInfo info;
        private bool damage_taken;
        private bool active;

        private void Awake()
        {
            info = GetComponent<UnitInfo>();
        }

        public void Recalc()
        {
            Max = info.Modules.OfType<IHeatContainer>().Sum(i => i.HeatCapacity);
            Dissipation = info.Modules.OfType<IHeatDissipator>().Sum(i => i.HeatDissipation);
            PassiveHeat = info.Modules.OfType<IPassiveHeatPoducer>().Sum(i => i.HeatPerRound);
        }

        public void Build(UnitTemplate template, PilotInfo pilot)
        {
            Threshold = 0.66f;
            Value = 0;
            Recalc();
                
            damage_taken = false;
            active = false;
        }

        public void BeginTurn()
        {
            damage_taken = false;
            active = true;
        }


        public void EndTurn()
        {
            if (info.Buffs.Shutdown)
            {
                Value -= Dissipation * 1.5f;
            }
            else
            {
                Value += PassiveHeat;
                Value -= Dissipation;
            }
            active = false;
            check_overheat();
        }

        public void AddHeat(float value)
        {
            Value += value;
            check_overheat();
        }

        private void check_overheat()
        {
            if (active)
                return;

            if(Value > Max)
            {
                info.UnitHP.DoOverheatDamage();

                if(!damage_taken)
                    info.UnitHP.DoOverheatDamage();

                damage_taken = true;

                info.Shutdown();
            }
            else if(Value > ThresholdValue && !damage_taken)
            {
                info.UnitHP.DoOverheatDamage();
                damage_taken = true;
            }
            

            Value = Mathf.Clamp(Value, 0, Max);

        }
    }
}
