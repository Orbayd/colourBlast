using System.Collections;
using System.Collections.Generic;
using ColourBlast.Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "GridConfig", menuName = "ScriptableObjects/GridConfig", order = 1)]
public class BlastGridConfig : ScriptableObject
{
    [Range(2,10)]
    public int ColumnLenght;
    [Range(2,10)]
    public int RowLenght;
    public float CellSize;
    public bool IsFixedSize;
    public bool IsFlexible;
    public GridAnchorPosition AnchorPosition;

}
