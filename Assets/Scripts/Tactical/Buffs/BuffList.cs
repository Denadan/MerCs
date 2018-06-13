
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mercs.Tactical.Buffs
{
    public class BuffList : MonoBehaviour, IEnumerable<BuffDescriptor>
    {
        private List<BuffDescriptor> buffs = new List<BuffDescriptor>();

        public void EndPhase()
        {

        }

        public void EndRound()
        {

        }

        public void BeginTurn()
        {

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
    }
}
