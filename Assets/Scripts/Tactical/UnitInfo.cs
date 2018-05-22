using UnityEngine;

namespace Mercs.Tactical
{
    [RequireComponent(typeof(CellPosition))]
    [RequireComponent(typeof(MovementData))]
    public class UnitInfo : MonoBehaviour
    {
        public CellPosition Position { get; set; }
        public MovementData Movement { get; set; }

        private void Start()
        {
            Position = GetComponent<CellPosition>();
            Movement = GetComponent<MovementData>();
        }
    }

}