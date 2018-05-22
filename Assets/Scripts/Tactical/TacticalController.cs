using System.Collections.Generic;
using Mercs.Tactical.Events;
using Tools;
using UnityEngine;


namespace Mercs.Tactical
{
    [RequireComponent(typeof(TileSubscriber))]
    public class TacticalController : SceneSingleton<TacticalController>
    {
        public Map Map { get; set; }
        public HexGrid Grid { get; set; }
        public List<UnitInfo> Units = new List<UnitInfo>();

        public GameObject MapPrefab;

        public void Start()
        {
            Instantiate(MapPrefab);
            Map = FindObjectOfType<Map>();
            Grid = FindObjectOfType<HexGrid>();
            Units.Clear();
        }
    }
}