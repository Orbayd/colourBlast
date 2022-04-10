
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
                    var blastItem = _factory.CreateRandom();
                    grid.SetCell(row, column.Key, blastItem);

                }
            }
        }
        _emptyGroups.Clear();
    }
}