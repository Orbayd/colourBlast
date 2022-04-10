
using System.Collections.Generic;
using System.Linq;

public class BlastGridGrouper
{
    public List<BlastGroup> _blastGroups;
    public BlastGridGrouper(List<BlastGroup> blastGroups)
    {
        _blastGroups = blastGroups;
    }
    public void CreateBlastableGroups(AnimatedBlastGrid2D<BlastItem> _grid)
    {
        _blastGroups.Clear();
        _grid.TraverseAll((row, column) =>
       {
           var blastgroup = Find(row, column);
           if (blastgroup is null)
           {
               //If not create new Group
               var value = _grid.GetCell(row, column);
               blastgroup = new BlastGroup() { Value = value.BlastColour };
               blastgroup.Add(row, column);
               _blastGroups.Add(blastgroup);
               CreateBlastableGroupsInner(row, column, _grid, blastgroup);
           }
       });

    }

    private void CreateBlastableGroupsInner(int row, int column, AnimatedBlastGrid2D<BlastItem> _grid, BlastGroup group)
    {
        if (row < _grid.RowLenght - 1)
        {
            var rowNeigbor = _grid.GetCell(row + 1, column);
            if (group.Value == rowNeigbor.BlastColour)
            {
                if (!group.Contains(row + 1, column))
                {
                    group.Add(row + 1, column);
                    CreateBlastableGroupsInner(row + 1, column, _grid, group);
                }
            }

        }
        if (column < _grid.ColumnLenght - 1)
        {
            var columnNeigbor = _grid.GetCell(row, column + 1);
            if (group.Value == columnNeigbor.BlastColour)
            {
                if (!group.Contains(row, column + 1))
                {
                    group.Add(row, column + 1);
                    CreateBlastableGroupsInner(row, column + 1, _grid, group);
                }
            }
        }
        if (row > 0)
        {
            var rowNeigbor = _grid.GetCell(row - 1, column);
            if (group.Value == rowNeigbor.BlastColour)
            {
                if (!group.Contains(row - 1, column))
                {
                    group.Add(row - 1, column);
                    CreateBlastableGroupsInner(row - 1, column, _grid, group);
                }
            }
        }

        if (column > 0)
        {
            var columnNeigbor = _grid.GetCell(row, column - 1);
            if (group.Value == columnNeigbor.BlastColour)
            {
                if (!group.Contains(row, column - 1))
                {
                    group.Add(row, column - 1);
                    CreateBlastableGroupsInner(row, column - 1, _grid, group);
                }
            }
        }
    }
    
    public BlastGroup Find(int row, int column)
    {
        return _blastGroups.FirstOrDefault(x => x.Contains(row, column));
    }
}