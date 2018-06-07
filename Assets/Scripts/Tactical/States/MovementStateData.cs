using System.Collections.Generic;

namespace Mercs.Tactical.States
{
    /// <summary>
    /// данные для задания движения
    /// </summary>
    public class MovementStateData
    {
        /// <summary>
        /// целевой тайл
        /// </summary>
        public PathMap.path_target target { get; set; }
        /// <summary>
        /// найденный путь
        /// </summary>
        public List<PathMap.path_node> path { get; set; }
        /// <summary>
        /// нужное направление по окончании движения
        /// </summary>
        public Dir dir { get; set; }
    }
}