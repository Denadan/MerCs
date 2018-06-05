using System.Collections.Generic;
using System.Linq;


namespace Mercs.Tactical.States
{
    public class PhasePrepareState : TacticalStateHandler
    {
        public override TacticalState State => TacticalState.PhasePrepare;

        public List<UnitInfo> ActiveUnits;


        public override void OnLoad()
        {
            while(TacticalController.Instance.CurrentPhase < 5)
            { 
                TacticalController.Instance.CurrentPhase += 1;

                ActiveUnits = (from unit in TacticalController.Instance.Units
                               where unit.Active  && (TacticalController.Instance.CurrentPhase == 5
                                || unit.Movement.Initiative <= TacticalController.Instance.CurrentPhase)
                               select unit).ToList();

                TacticalUIController.Instance.RoundText = $"Round {TacticalController.Instance.CurrentRound}-{TacticalController.Instance.CurrentPhase}";

                if (ActiveUnits.Count != 0)
                {
                    TacticalController.Instance.StateMachine.State = TacticalState.PhaseSelectFaction;
                    foreach (var unitInfo in TacticalController.Instance.Units.Where(
                        unit => unit.Faction == GameController.Instance.PlayerFaction 
                                && ActiveUnits.Contains(unit)))
                    {
                        unitInfo.Selectable = true;
                    }
                    return;
                }
            }
            SwitchTo(TacticalState.TurnPrepare);
        }
    }
}
