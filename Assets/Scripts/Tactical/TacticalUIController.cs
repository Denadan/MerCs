using UnityEngine;
using Tools;
using UnityEngine.UI;
using Mercs.Tactical.UI;
using Mercs.Tactical.Events;

namespace Mercs.Tactical
{
    public class TacticalUIController : SceneSingleton<TacticalUIController>
    {
        [Header("UnitListMenu")]
        [SerializeField]
        private GameObject UnitListMenu;
        [SerializeField]
        private GameObject UnitListItemPrefab;
        [Header("TopButtons")]
        [SerializeField]
        private Button StartButton;
        [SerializeField]
        private Button DoneButon;
        [SerializeField]
        private Button ReserveButton;
        [Header("Other")]
        [SerializeField]
        private GameObject TileInfoMenu;
        [SerializeField]
        private DeployInfoMenu DeployWindow;

        [SerializeField]
        public DebugMapMenu DebugMenu;

        public LineRenderer RotationLine;


        public void ClearUnitList()
        {
            foreach(Transform child in UnitListMenu.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public UnitSelectButton AddUnit(UnitInfo info)
        {
            var item = Instantiate(UnitListItemPrefab, UnitListMenu.transform, false).GetComponent<UnitSelectButton>();

            item.Unit = info;
            item.GetComponent<UnitEventRelay>().OriginalUnit = info;
            item.BottomText.text = "";

            return item;
        }

        public void ShowUnitList()
        {
            UnitListMenu.SetActive(true);
        }

        public void HideUnitList()
        {
            UnitListMenu.SetActive(false);
        }

        public void DisableStartButton()
        {
            StartButton.interactable = false;

        }

        public void InitDeployWindow()
        {
            StartButton.gameObject.SetActive(true);
            DeployWindow.gameObject.SetActive(true);
            StartButton.interactable = DeployWindow.InitDeployInfo(GameController.Instance.DeployLimit);
        }

        public void UpdateDeployWindow()
        {
            StartButton.interactable = DeployWindow.UpdateDeployInfo();
        }

        public void HideDeployWindow()
        {
            StartButton.gameObject.SetActive(false);
            DeployWindow.gameObject.SetActive(false);
            DeployWindow.InitDeployInfo(GameController.Instance.DeployLimit);
        }

    }
}
