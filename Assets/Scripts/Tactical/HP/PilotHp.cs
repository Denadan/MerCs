using System.Collections.Generic;
using Mercs.Tactical.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mercs.Tactical
{

    public class PilotHp : MonoBehaviour
    {
        public int MaxHp { get; private set; }
        public int Hp { get; private set; }
        public int AddHp { get; private set; }

        private UnitInfo unit;

        private List<GameObject> subscribers = new List<GameObject>();


        public void Init(StartMechInfo info)
        {
            MaxHp = info.Pilot.Hp;
            Hp = MaxHp;
            AddHp = info.Merc.AddHp;
        }

        public void Start()
        {
            unit = GetComponent<UnitInfo>();
            EventHandler.RegisterPilotHp(unit, this);
        }

        public void DoDamage(int damage)
        {
            if (AddHp > 0)
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

            foreach (var subscriber in subscribers)
                ExecuteEvents.Execute<IPilotDamaged>(subscriber, null,
                    (handler, eventData) => handler.PilotDamaged(this));
        }

        private void OnDestroy()
        {
            subscribers.Clear();
            EventHandler.UnRegisterPilotHp(unit);
        }

        public void Subscribe(GameObject go)
        {
            subscribers.Add(go);
        }

        public void Unsubscribe(GameObject go)
        {
            subscribers.Remove(go);
        }

    }
}