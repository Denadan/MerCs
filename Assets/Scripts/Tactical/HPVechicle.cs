using System.Linq;

namespace Mercs.Tactical
{
    public class HPVechicle : HpTable
    {
        public int Struct;
        public int[] Armor;


        public override void Hit(Dir HitDirection)
        {
            
        }

        public override int PartsCount => Armor == null ? 0 : Armor.Length;

        public override (int, int) this[int partNum] =>
            (Armor == null || partNum > Armor.Length) ? (0, 0) : (Armor[partNum], Struct);
        
        public override (int, int) Total => (Armor?.Sum() ?? 0, Struct);
    }
}