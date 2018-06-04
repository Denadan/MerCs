
using System.Linq;

namespace Mercs.Tactical.States
{
    public class TurnPrepareState : TacticalStateHandler
    {
        public override TacticalState State => TacticalState.TurnPrepare;

        public override void Update()
        {
            foreach(var unit in TacticalController.Instance.Units.Where(i => !i.Reserve))
            {
                unit.Movement.NewTurn();
                unit.Active = true;
            }

            TacticalController.Instance.CurrentRound += 1;
            TacticalController.Instance.CurrentPhase = 0;

            SwitchTo(TacticalState.PhasePrepare);
        }
    }
}
