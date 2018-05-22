using UnityEngine;

namespace Mercs.Tactical
{
    public class MovementData : MonoBehaviour
    {
        public int MoveMp;
        public int RunMP;
        public int JumpMP;

        public int CurrentMP { get; private set; }
        public bool Runnig { get; private set; }


        private bool moved;
        public void SetRun()
        {
            if (moved)
                return;
            Runnig = true;
            CurrentMP = RunMP;
        }

        public void SetMove()
        {
            if (moved)
                return;
            Runnig = false;
            CurrentMP = MoveMp;
        }

        public void NewTurn()
        {
            moved = false;
            CurrentMP = MoveMp;
            Runnig = false;
        }

        public void SpendMP(int count)
        {
            if(count != 0)
            {
                moved = true;
                CurrentMP = CurrentMP < count ? 0 : CurrentMP - count;
            }
        }

        public void Jump()
        {
            CurrentMP = 0;
        }
    }
}
