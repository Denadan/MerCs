using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercs.Tactical.States
{
    public class PhaseSelectFactionState : TacticalStateHandler
    {
        private PhasePrepareState state;

        public override TacticalState State => TacticalState.PhaseSelectFaction;

        public PhaseSelectFactionState(PhasePrepareState state)
        {
            this.state = state;
        }

        public override void OnLoad()
        {
            Faction faction;
            

            do
            {
                 faction = TacticalController.Instance.NextFaction();
            } while (state.ActiveUnits.Find(item => item.Faction == faction) == null);

            if (faction == GameController.Instance.PlayerFaction)
            {
                SwitchTo(TacticalState.PlayerPrepare);
            }
            else
            {
                SwitchTo(TacticalState.AIPrepare);
            }
        }
    }
}
