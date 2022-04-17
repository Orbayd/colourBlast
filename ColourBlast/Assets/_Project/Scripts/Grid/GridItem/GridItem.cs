using ColourBlast.Grid2D;
using UnityEngine;

namespace ColourBlast
{
    public abstract class GridItem : MonoBehaviour, IGridItem
    {
        public CellPosition Position { get; set; }
    }
}
