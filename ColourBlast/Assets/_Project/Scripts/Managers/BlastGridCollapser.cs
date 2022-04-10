
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlastGridCollapser
{   
    private List<BlastGroup> _emptyGroups;
    public BlastGridCollapser(List<BlastGroup> emptyGroups)
    {
        _emptyGroups = emptyGroups;
    }
    public void Collapse(AnimatedBlastGrid2D<BlastItem> grid)
    {
        var emptyCells = grid.GetEmptyCells();
        var blastGroup = new BlastGroup();
        foreach (var emptyCell in emptyCells)
        {
            blastGroup.Add(emptyCell.Row,emptyCell.Column);
        }
        
        Collapse(grid,blastGroup);
    }
    public void Collapse(AnimatedBlastGrid2D<BlastItem> grid, BlastGroup group)
    {
        var colums = group.GetColumns();
        foreach (var column in colums)
        {
            var emptyGroup = new BlastGroup();
            _emptyGroups.Add(emptyGroup);
            Debug.Log("Collapsing");
            Debug.Log($"Column[{column.Key}] Rows: " + string.Join(",", column.Value.Select(x => x.ToString())));
            var rowIds = column.Value.OrderByDescending(x => x);

            var maxRange = rowIds.First();
            var minRange = rowIds.Last();
            for (int i = maxRange; i >= minRange; i--)
            {
                if (grid.GetCell(i, column.Key) != null)
                {
                    GameObject.Destroy(grid.GetCell(i, column.Key).gameObject);
                    grid.SetCell(i, column.Key, null);
                }
                emptyGroup.Add(i, column.Key);
            }

            if (minRange > 0)
            {
                for (int i = minRange - 1; i >= 0; i--)
                {
                    if (grid.GetCell(i + (maxRange - minRange + 1), column.Key) != null)
                    {
                        GameObject.Destroy(grid.GetCell(i + (maxRange - minRange + 1), column.Key).gameObject);
                    }
                    var value = grid.GetCell(i, column.Key);
                    if (value != null)
                    {
                        grid.SetCell(i + (maxRange - minRange + 1), column.Key, value);
                        grid.SetCell(i, column.Key, null);
                    }
                    emptyGroup.Add(i, column.Key);
                    if (emptyGroup.Contains(i + (maxRange - minRange + 1), column.Key))
                    {
                        emptyGroup.Remove(i + (maxRange - minRange + 1), column.Key);
                    }
                }
            }
        }
    }

}