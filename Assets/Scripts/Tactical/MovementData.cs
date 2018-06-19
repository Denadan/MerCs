using System.Linq;
using Mercs.Items;
using Mercs.Tactical.Buffs;
using UnityEngine;

namespace Mercs.Tactical
{
    public class MovementData : MonoBehaviour
    {
        private int move, run, jump, leg_jump;
        private int initiative;
        private bool evasive;

        // TODO: сделать проверки пилота!
        /// <summary>
        /// может ли в уклонение 
        /// </summary>
        public bool CanDoEvasive
        {
            get => evasive;
            set => evasive = value;
        }

        public int MoveMp
        {
            get => info.Buffs.Shutdown ? 0 : move;
            set => move = value;
        }
        public int RunMP
        {
            get => info.Buffs.Shutdown ? 0 : run;
            set => run = value;
        }
        public int JumpMP
        {
            get => info.Buffs.Shutdown ? 0 : jump;
            set => jump = value;
        }

        public int Initiative
        {
            get => initiative - (int)info.Buffs.SumBuffs(BuffType.InitiativeBonus);
            set => initiative = value;
        }

        public bool CanMove { get; private set; }

        private UnitInfo info;

        private void Awake()
        {
            info = GetComponent<UnitInfo>();
        }

        public void NewTurn()
        {
            CanMove = true;
        }

        public void MoveDone()
        {
            CanMove = false;
        }

        public void Build()
        {
            move = (int)(info.Modules.Reactor.EngineRating *
                        info.Modules.OfType<IMoveMod>().Aggregate(1f, (i, mod) => i * mod.MoveMod)
                         / info.Weight);
            run = (int)(info.Modules.Reactor.EngineRating *
                        info.Modules.OfType<IRunMod>().Aggregate(1f, (i, mod) => i * mod.RunMod)
                        * 1.5f / info.Weight);

            var jump_mod = info.Modules.OfType<IJumpMod>().Aggregate(1f, (i, mod) => i * mod.JumpMod);
            jump = (int)(info.Modules.OfType<JumpJet>().Sum(i => i.EngineRating) * jump_mod / info.Weight);

            if (info.UnitHP.AllParts.Contains(Parts.LL) && info.UnitHP.AllParts.Contains(Parts.RL))
            {
                var jump_in_legs = info.Modules[Parts.LL].OfType<JumpJet>().Sum(i => i.EngineRating)
                    + info.Modules[Parts.RL].OfType<JumpJet>().Sum(i => i.EngineRating);

                leg_jump = (int)(jump_in_legs * jump_mod / info.Weight);
            }
            else
            {
                leg_jump = 0;
            }
            Initiative = CONST.Initiative(info.Class);
        }
    }
}
