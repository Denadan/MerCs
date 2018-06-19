using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mercs.Tactical
{
    public class HeatData : MonoBehaviour
    {
        public float Value { get; private set; }
        public float Max { get; private set; }

        private UnitInfo info;

        private void Awake()
        {
            info = GetComponent<UnitInfo>();
        }

        public void Build(UnitTemplate template, PilotInfo pilot)
        {

        }

        public void Subscribe(GameObject go)
        {

        }

        public void UnSubscribe(GameObject go)
        {

        }
    }
}
