
using System.Collections.Generic;
using System.Linq;

public class BlastGridFiller
{
    private List<BlastGroup> _emptyGroups;
    private BlastItemFactory _factory;
    public BlastGridFiller(List<BlastGroup> emptyGroups,BlastItemFactory factory)
    {
        _emptyGroups = emptyGroups;
        _factory = factory;
    }
    public void Fill(AnimatedBlastGrid2D<BlastItem> grid)
    {
        foreach (var group in _emptyGroups)
        {
            var columns = group.GetColumns();
            foreach (var column in columns)
            {
                var rowIds = column.Value.OrderByDescending(x => x);
                foreach (var row in rowIds)
                {
                    var blastItem =  _factory.CreateRandom();
                    blastItem.transform.position = grid.GridToWorldPosition(row,column.Key);
                    grid.SetCell(row, column.Key,blastItem);
                }
            }
        }
        _emptyGroups.Clear();
    }

    public void Fill(AnimatedBlastGrid2D<BlastItem> grid, AnimatedBlastGrid2D<BlastItem> reserveGrid)
    {
        foreach (var group in _emptyGroups)
        {
            var columns = group.GetColumns();
            foreach (var column in columns)
            {
                var rowIds = column.Value.OrderByDescending(x => x);
                foreach (var row in rowIds)
                {
                    var blastItem = reserveGrid.RemoveLastItemInColumn(column.Key);
                    grid.SetCell(row, column.Key, blastItem);
                }
            }
        }
        _emptyGroups.Clear();
    }
}