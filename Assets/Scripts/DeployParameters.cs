using System;
using UnityEngine;

namespace Mercs
{
    [Serializable]
    public class DeployParameters 
    {
        [Flags]
        public enum Range { None = 0, Min = 1, Max = 2, Between = 3 }

        public Range CheckCount = Range.None;
        public Vector2Int CountLimit;

        public Range CheckWeight = Range.None;
        public Vector2Int WeighLimit;
    }
}