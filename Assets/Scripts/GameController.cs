using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace Mercs
{
    public class GameController : Singleton<GameController>
    {
        [Header("TacticalMapInfo")]
        public List<StartMechInfo> Mechs = new List<StartMechInfo>();
    }
}