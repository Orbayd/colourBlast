using UnityEngine;

public abstract class GridItem : MonoBehaviour, IGridItem
{
    public CellPosition Position { get ; set; }
}

