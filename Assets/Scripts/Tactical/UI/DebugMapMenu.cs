using UnityEngine;
using UnityEngine.UI;

namespace Mercs.Tactical.UI
{
    public class DebugMapMenu : MonoBehaviour
    {
        public Text Rotation;

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