using UnityEngine;

namespace Mercs.Tactical.UI
{
    public class DebugMapMenu : MonoBehaviour
    {
        private PlaceUnit unitPlacer;

        public void ShowPath(bool value)
        { }

        public void ShowLink(bool value)
        { }

        public void ResetMap()
        {
            TacticalController.Instance.Grid.gameObject.SendMessage("Clear");
            TacticalController.Instance.Grid.gameObject.SendMessage("Start");

        }

        public void AddUnit()
        {
            TacticalController.Instance.Map.GetComponent<PlaceUnit>().Place();

        }
    }
}