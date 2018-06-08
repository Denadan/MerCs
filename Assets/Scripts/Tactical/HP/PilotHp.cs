using Mercs.Tactical.Events;
using UnityEngine;

namespace Mercs.Tactical
{
    
    public class PilotHp : MonoBehaviour
    {
        public int MaxHp { get; private set; }
        public int Hp { get; private set; }
        public int AddHp { get; private set; }

        private UnitInfo unit;

        public void Init(StartMechInfo info)
        {
            MaxHp = info.Pilot.Hp;
            Hp = MaxHp;
            AddHp = info.Merc.AddHp;
        }

        public void Start()
        {
            unit = GetComponent<UnitInfo>();
        }

        public void DoDamage(int damage)
        {
           if(AddHp > 0)
                if (damage > AddHp)
                {
                    damage -= AddHp;
                    AddHp = 0;
                }
                else
                {
                    AddHp -= damage;
                    damage = 0;
                }

            if (damage > 0)
                if (Hp > damage)
                    Hp -= damage;
                else
                    Hp = 0;

            EventHandler.PilotHpChange(unit);
        }
    }
}