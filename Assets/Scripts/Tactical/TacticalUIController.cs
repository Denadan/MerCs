#pragma warning disable 649

using UnityEngine;
using Tools;
using UnityEngine.UI;
using Mercs.Tactical.UI;
using Mercs.Tactical.Events;
using System.Collections.Generic;

namespace Mercs.Tactical
{
    public class TacticalUIController : SceneSingleton<TacticalUIController>
    {
        private Dictionary<ActionButton, Image> action_bar;
        private Dictionary<TacticalButton, Button> tactical_button;
        private Image highlated_abb;

        [Header("UnitListMenu")]
        [SerializeField]
        private GameObject UnitListMenu;
        [SerializeField]
        private GameObject UnitListItemPrefab;
        [Header("TopButtons")]
        [SerializeField]
        private Button StartButton;
        [SerializeField]
        private Button DoneButton;
        [SerializeField]
        private Button ReserveButton;
        [SerializeField]
        private Button ConfirmButton;
        [Header("Action Bar")]
        [SerializeField]
        private RectTransform ActionBar;

        [SerializeField]
        private Image ABB_Move;
        [SerializeField]
        private Image ABB_Run;
        [SerializeField]
        private Image ABB_Jump;
        [SerializeField]
        private Image ABB_Guard;
        [SerializeField]
        private Image ABB_Cancel;


        [Header("Other")]
        [SerializeField]
        private GameObject TileInfoMenu;
        [SerializeField]
        private DeployInfoMenu DeployWindow;
        [SerializeField]
        private UnitStateWindow SelectedUnitStateWindow;
        [SerializeField]
        private Text roundText;

        [SerializeField]
        public DebugMapMenu DebugMenu;

        public LineRenderer MoveLine;


        public string RoundText
        {
            set => roundText.text = value;
        }

        private void Start()
        {
            action_bar = new Dictionary<ActionButton, Image>()
            {
                [ActionButton.Move] = ABB_Move,
                [ActionButton.Jump] = ABB_Jump,
                [ActionButton.Run] = ABB_Run,
                [ActionButton.Guard] = ABB_Guard,
                [ActionButton.Cancel] = ABB_Cancel,
            };
            tactical_button = new Dictionary<TacticalButton, Button>
            {
                [TacticalButton.Confirm] = ConfirmButton,
                [TacticalButton.Done] = DoneButton,
                [TacticalButton.Reserve] = ReserveButton,
            };
        }

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
        
        public void ShowActionBar()
        {
            ActionBar.gameObject.SetActive(true);
        }

        public void HideActionBar()
        {
            ActionBar.gameObject.SetActive(false);
        }

        public void ShowActionBarButton(ActionButton button)
        {
            action_bar[button].gameObject.SetActive(true);
        }

        public void ShowActionBarButtons(UnitInfo unit)
        {
            if (unit.Movement.MoveMp > 0)
                ShowActionBarButton(ActionButton.Move);
            else
                HideActionBarButton(ActionButton.Move);

            if (unit.Movement.RunMP > 0)
                ShowActionBarButton(ActionButton.Run);
            else
                HideActionBarButton(ActionButton.Run);

            if (unit.Movement.JumpMP > 0)
                ShowActionBarButton(ActionButton.Jump);
            else
                HideActionBarButton(ActionButton.Jump);

            ShowActionBarButton(ActionButton.Guard);
        }

        public void HideActionBarButton(ActionButton button)
        {
            action_bar[button].gameObject.SetActive(false);
        }

        public void HighlightActionBarButton(ActionButton button)
        {
            if (highlated_abb != null)
                highlated_abb.color = Color.white;

            highlated_abb = action_bar[button];

            if (highlated_abb != null)
                highlated_abb.color = Color.yellow;
        }

        public void ShowButton(TacticalButton button)
        {
            tactical_button[button].gameObject.SetActive(true);
        }
        public void HideButton(TacticalButton button)
        {
            tactical_button[button].gameObject.SetActive(false);
        }

        public void MoveCameraTo(UnitInfo selectedUnit)
        {
            Camera.main.transform.position = new Vector3(selectedUnit.gameObject.transform.position.x, selectedUnit.gameObject.transform.position.y,
                Camera.main.transform.position.z);
        }

        public void HideSelectedUnitWindow()
        {
            SelectedUnitStateWindow.gameObject.SetActive(false);
        }

        public void ShowSelectedUnitWindow(UnitInfo value)
        {
            SelectedUnitStateWindow.gameObject.SetActive(true);
            SelectedUnitStateWindow.SetUnit(value);
        }
    }
}
