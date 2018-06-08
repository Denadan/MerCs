using System;


namespace Mercs.Tactical
{
    [Serializable]
    public class UnitPart
    {
        public Parts Place;

        public float Structure;
        public float Armor;
        public float BackArmor;

        public Parts DependOn = Parts.None;
        public Parts TransferTo = Parts.None;

        public bool Critical = false;
        public bool HasBackArmor = false;

        public float[] Size = { 0, 0, 0, 0 };
#if UNITY_EDITOR
        public void SetDefaultSize()
        {
            switch (Place)
            {
                case Parts.HD:
                    Size = new float[] { 1, 1, 1, 0 };
                    break;
                case Parts.CT:
                    Size = new float[] { 7, 5, 5, 7 };
                    break;
                case Parts.LT:
                    Size = new float[] { 5, 7, 0, 5 };
                    break;
                case Parts.RT:
                    Size = new float[] { 5, 0, 7, 5 };
                    break;
                case Parts.LH:
                    Size = new float[] { 5, 7, 0, 5 };
                    break;
                case Parts.RH:
                    Size = new float[] { 5, 0, 7, 5 };
                    break;
                case Parts.LS:
                    Size = new float[] { 3, 4, 0, 3 };
                    break;
                case Parts.RS:
                    Size = new float[] { 3, 0, 4, 3 };
                    break;
                case Parts.LL:
                    Size = new float[] { 4, 7, 0, 4};
                    break;
                case Parts.RL:
                    Size = new float[] { 4, 0, 7, 4 };
                    break;

                case Parts.FC:
                    Size = new float[] { 5, 2, 2, 0 };
                    break;
                case Parts.BC:
                    Size = new float[] { 0, 2, 2, 5 };
                    break;
                case Parts.LC:
                    Size = new float[] { 2, 5, 0, 2 };
                    break;
                case Parts.RC:
                    Size = new float[] { 2, 0, 5, 2 };
                    break;

                case Parts.TC:
                    Size = new float[] { 1, 1, 1, 1 };
                    break;

                case Parts.VC:
                    Size = new float[] { 1, 1, 1, 0 };
                    break;
                case Parts.LM:
                    Size = new float[] { 1, 2, 2, 2 };
                    break;

                case Parts.TT:
                    Size = new float[] {1, 1, 1, 1 };
                    break;

            }
        }
#endif
    }
}
