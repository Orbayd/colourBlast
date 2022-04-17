
using System.Collections.Generic;
using System.Linq;
using ColourBlast.Grid2D;
using ColourBlast.Helpers;
using UnityEngine;

public class BlastGridCollapser : ICollapseCommand
{
    public PoolingService _poolService;
    public BlastGridCollapser(PoolingService poolService)
    {
        _poolService = poolService;
    }

    public void Collapse(AnimatedBlastGrid2D<BlastItem> grid, IEnumerable<CellPosition> source)
    {
        var colums = grid.GroupByColumns(source);
        foreach (var column in colums)
        {
            var rowIds = column.Value.OrderByDescending(x => x);

            var maxRange = rowIds.First();
            var minRange = rowIds.Last();
            for (int i = maxRange; i >= minRange; i--)
            {
                if (grid.GetCell(i, column.Key) != null)
                {
                    //GameObject.Destroy(grid.GetCell(i, column.Key).gameObject);
                    _poolService.Release(grid.GetCell(i, column.Key).gameObject);
                    grid.SetEmpty(i, column.Key);
                }
            }

            if (minRange > 0)
            {
                for (int i = minRange - 1; i >= 0; i--)
                {
                    if (grid.GetCell(i + (maxRange - minRange + 1), column.Key) != null)
                    {
                        //GameObject.Destroy(grid.GetCell(i + (maxRange - minRange + 1), column.Key).gameObject);
                        _poolService.Release(grid.GetCell(i + (maxRange - minRange + 1), column.Key).gameObject);
                    }
                    var value = grid.GetCell(i, column.Key);
                    if (value != null)
                    {
                        grid.SetCell(i + (maxRange - minRange + 1), column.Key, value);
                        grid.SetEmpty(i, column.Key);
                    }
                }
            }
        }
    }

}