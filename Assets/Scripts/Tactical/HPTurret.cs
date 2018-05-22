namespace Mercs.Tactical
{
    public class HPTurret : HpTable
    {
        public int Armor;
        public int Sturcture;

        public override void Hit(Dir HitDirection)
        {
            
        }

        public override int PartsCount => 1;

        public override (int, int) this[int partNum] => (Armor, Sturcture);

        public override (int, int) Total => (Armor, Sturcture);
    }
}