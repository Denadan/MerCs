using UnityEngine;

namespace Mercs.Tactical
{
    public class CellPosition : MonoBehaviour
    {
        public Vector2Int position;
        public Dir Facing = Dir.N;

        public void SetFacing(Dir dir)
        {
            Facing = dir;
            this.transform.rotation = Quaternion.Euler(0,0,CONST.GetAngleV(dir));
        }
    }
}
