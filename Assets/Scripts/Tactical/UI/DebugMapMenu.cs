using UnityEngine;

namespace Mercs.Tactical.UI
{
    public class DebugMapMenu : MonoBehaviour
    {
        public void ShowPath(bool value)
        { }

        public void ShowLink(bool value)
        { }

        public void ResetMap()
        {
            TacticalController.Instance.Grid.gameObject.SendMessage("Clear");
            TacticalController.Instance.Grid.gameObject.SendMessage("Start");

        }

    }
}