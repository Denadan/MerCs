using UnityEngine;

namespace Mercs
{
    public class PilotInfo : ScriptableObject
    {
        public string Name;
        public int InitiativeBonus;
        public int Hp;
        public bool Evasive;

        public int VisualBonus;
    }
}
