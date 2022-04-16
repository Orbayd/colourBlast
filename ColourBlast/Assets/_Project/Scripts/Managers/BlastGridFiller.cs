
using System.Collections.Generic;
using System.Linq;

public class BlastGridFiller
{
    //private List<BlastGroup> _emptyGroups;
    public List<CellPosition> _emptyPositions;
    private BlastItemFactory _factory;
    public BlastGridFiller(BlastItemFactory factory)
    {
    
        _factory = factory;
    }
    public void Fill(AnimatedBlastGrid2D<BlastItem> grid)
    {
        _emptyPositions = grid.GetEmptyCells().ToList();
        var columns = grid.GroupByColumns(_emptyPositions);
        foreach (var column in columns)
        {
            var rowIds = column.Value.OrderByDescending(x => x);
            foreach (var row in rowIds)
            {
                var blastItem = _factory.CreateRandom();
                blastItem.transform.position = grid.GridToWorldPosition(row, column.Key);
                grid.SetCell(row, column.Key, blastItem);
            }
        }
    }

    public void Fill(AnimatedBlastGrid2D<BlastItem> grid, AnimatedBlastGrid2D<BlastItem> reserveGrid)
    {
        _emptyPositions = grid.GetEmptyCells().ToList();
        var columns = grid.GroupByColumns(_emptyPositions);
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
}