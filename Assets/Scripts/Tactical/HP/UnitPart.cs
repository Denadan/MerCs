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
    }
}
