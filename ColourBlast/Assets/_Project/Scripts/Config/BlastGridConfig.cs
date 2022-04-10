using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridConfig", menuName = "ScriptableObjects/GridConfig", order = 1)]
public class BlastGridConfig : ScriptableObject
{
    [Min(1)]
    public int ColumnLenght;
    [Min(1)]
    public int RowLenght;

    public float CellSize;
}
