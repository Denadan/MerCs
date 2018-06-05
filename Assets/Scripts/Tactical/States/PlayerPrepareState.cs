using System.Linq;
using UnityEngine;

namespace Mercs.Tactical.States
{
    public class PlayerPrepareState : TacticalStateHandler
    {
        private PhasePrepareState state;
        private Color grey, dark_grey;

        public override TacticalState State => TacticalState.PlayerPrepare;

        public PlayerPrepareState(PhasePrepareState state)
        {
            this.state = state;
            grey = new Color(0.66f, 0.66f, 0.66f, 1);
            dark_grey = new Color(0.33f, 0.33f, 0.33f, 1);
        }

        public override void OnLoad()
        {

            int n = 0;
            foreach (var i in TacticalController.Instance.PlayerUnits.
                Select(unit => (unit: unit, button: TacticalController.Instance[unit])).
                Where(item => item.button != null))
            {
                //UnityEngine.Debug.Log($"{i.unit.PilotName} active:{i.unit.Active} selectable:{i.unit.Selectable}");

                if (i.unit.Active)
                    if (i.unit.Selectable)
                    {
                        i.button.Background.color = Color.white;
                        n += 1;
                    }
                    else
                        i.button.Background.color = grey;
                else
                    i.button.Background.color = dark_grey;
            }

            var selected = state.ActiveUnits.First(unit => unit.Selectable);

            if (n == 1)
            {
                TacticalController.Instance.SelectedUnit = selected;
                TacticalUIController.Instance.MoveCameraTo(state.ActiveUnits.First(unit => unit.Selectable));
                TacticalController.Instance[selected].Background.color = Color.green;
                SwitchTo(TacticalState.SelectMovement);
            }
            else
            {
                TacticalUIController.Instance.MoveCameraTo(selected);
                SwitchTo(TacticalState.SelectUnit);
            }

        }
    }
}
