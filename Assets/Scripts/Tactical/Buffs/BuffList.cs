
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mercs.Tactical.Buffs
{
    public class BuffList : MonoBehaviour, IEnumerable<BuffDescriptor>
    {
        public UnitInfo info;

        private List<BuffDescriptor> buffs = new List<BuffDescriptor>();

        public bool Overheated => info.Heat.Value > info.Heat.ThresholdValue;
        public bool Shutdown { get; private set; }
        public bool Prone { get; private set; }



        private void Awake()
        {
            info = GetComponent<UnitInfo>();
        }

        public void EndPhase()
        {

        }

        public void EndRound()
        {

        }

        public void BeginTurn()
        {
            buffs.RemoveAll(i => i.Duration == BuffDescriptor.BuffDuration.BeginNextTurn);
        }

        public void EndTurn()
        {

        }

      

        public void Add(BuffDescriptor buff)
        {
            buffs.Add(buff);
        }

        public void Remove(BuffDescriptor buff)
        {
            buffs.Remove(buff);
        }

        public IEnumerator<BuffDescriptor> GetEnumerator()
        {
            return buffs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return buffs.GetEnumerator();
        }

        public void EngineOff()
        {
            Shutdown = true;
        }

        public void EngineOn()
        {
            Shutdown = false;
        }
    }
}
