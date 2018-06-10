using Mercs.Tactical.Buffs;
using UnityEngine;

namespace Mercs.Tactical
{
    public class MovementData : MonoBehaviour
    {
        private int move, run, jump;
        private int initiative;

        public int MoveMp
        {
            get => move;
            set => move = value;
        }
        public int RunMP
        {
            get => run;
            set => run = value;
        }

        public int JumpMP
        {
            get => jump;
            set => jump = value;
        }

        public int Initiative
        {
            get => initiative - (int)unit.Buffs.SumBuffs(BuffType.InitiativeBonus);

            set => initiative = value;
        }

        private UnitInfo unit;

        private void Awake()
        {
            unit = GetComponent<UnitInfo>();
        }

        public void NewTurn()
        {
            
        }

    }
}
