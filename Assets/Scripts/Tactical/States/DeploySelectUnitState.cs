﻿using UnityEngine;
using UnityEngine.EventSystems;


namespace Mercs.Tactical.States
{
    public class DeploySelectUnitState : TacticalStateHandler
    {
        private DeployInitState state;
        public override TacticalState State => TacticalState.DeploySelectUnit;

        public DeploySelectUnitState(DeployInitState state)
        {
            this.state = state;
        }

        public override void UnitClick(UnitInfo unit, PointerEventData.InputButton button)
        {
            if (button == PointerEventData.InputButton.Left)
            {
                var unit_to_add = RemoveFromBoard(unit);
                if (unit_to_add == null)
                    return;
                state.unit_in_hand = unit_to_add;
                TacticalUIController.Instance.ShowSelectedUnitWindow(unit_to_add.info);
                unit_to_add.button.Background.color = Color.yellow;
                SwitchTo(TacticalState.DeployPlaceUnit);
            }
            else if (button == PointerEventData.InputButton.Right)
            {
                if (!unit.Reserve)
                    RemoveFromBoard(unit);
            }
        }

        private DeployInitState.unit RemoveFromBoard(UnitInfo unit)
        {
            var unit_to_remove = state.units.Find(u => u.info == unit);
            if (unit_to_remove == null)
                return null;

            if(!unit.Reserve)
                TacticalController.Instance.Overlay.ShowTile(unit_to_remove.Position.position, Color.yellow, MapOverlay.Texture.GridMark);

            unit_to_remove.info.gameObject.SetActive(false);
            unit_to_remove.Position.position = new Vector2Int(-1, -1);
            unit_to_remove.Position.SetFacing(Dir.N);
            unit_to_remove.info.Reserve = true;
            unit_to_remove.info.GFX.RemoveCollider();
            unit_to_remove.button.Background.color = Color.white;
            unit_to_remove.button.BottomText.text = "RESERVE";

            TacticalUIController.Instance.UpdateDeployWindow();
            return unit_to_remove;
        }

        public override void OnLoad()
        {
            state.ShowDeployZone();
            TacticalUIController.Instance.UpdateDeployWindow();
        }

        public override void OnUnload()
        {
            TacticalUIController.Instance.DisableStartButton();
        }
    }
}