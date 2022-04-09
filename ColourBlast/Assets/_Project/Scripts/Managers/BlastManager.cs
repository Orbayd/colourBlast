
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlastManager
{

    public List<BlastGroup> BlastGroups = new List<BlastGroup>();

    public List<BlastGroup> EmptyGroups = new List<BlastGroup>();

    public BlastGroup Find(int row, int column)
    {
        return BlastGroups.FirstOrDefault(x => x.Contains(row, column));
    }

    public void CreateBlastableGroups(BlastGrid2D<int> _grid)
    {
        BlastGroups.Clear();
        _grid.TraverseAll((row, column) =>
       {
           var blastgroup = Find(row, column);
           if (blastgroup is null)
           {
               //If not create new Group
               var value = _grid.GetCell(row, column);
               blastgroup = new BlastGroup() { Value = value };
               blastgroup.Add(row, column);
               BlastGroups.Add(blastgroup);
               CreateBlastableGroupsInner(row, column, _grid, blastgroup);

               if(!blastgroup.IsBlastable)
               {
                   BlastGroups.Remove(blastgroup);
               }
           }
       });
    }

    private void CreateBlastableGroupsInner(int row, int column, BlastGrid2D<int> _grid, BlastGroup group)
    {
        if (row < _grid.ColumnLenght - 1)
        {
            var rowNeigbor = _grid.GetCell(row + 1, column);
            if (group.Value == rowNeigbor)
            {
                if (!group.Contains(row + 1, column))
                {
                    group.Add(row + 1, column);
                    CreateBlastableGroupsInner(row + 1, column, _grid, group);
                }
            }

        }
        if (column < _grid.RowLenght - 1)
        {
            var columnNeigbor = _grid.GetCell(row, column + 1);
            if (group.Value == columnNeigbor)
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
            if (group.Value == rowNeigbor)
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
            if (group.Value == columnNeigbor)
            {
                if (!group.Contains(row, column - 1))
                {
                    group.Add(row, column - 1);
                    CreateBlastableGroupsInner(row, column - 1, _grid, group);
                }
            }
        }
    }

    public void Collapse(BlastGrid2D<int> _grid, int columnLenght, BlastGroup group)
    {
        var colums = group.GetColumns();
        foreach (var column in colums)
        {
            var emptyGroup = new BlastGroup();
            EmptyGroups.Add(emptyGroup);
            Debug.Log($"Column[{column.Key}] Rows: " + string.Join(",", column.Value.Select(x => x.ToString())));
            var rowIds = column.Value.OrderByDescending(x => x);

            var maxRange = rowIds.First();
            var minRange = rowIds.Last();
            for (int i = maxRange; i >= minRange; i--)
            {
                _grid.SetCell(i, column.Key, -1);
                emptyGroup.Add(i, column.Key);
            }

            if (minRange > 0)
            {
                for (int i = minRange - 1; i >= 0; i--)
                {
                    var value = _grid.GetCell(i, column.Key);
                    _grid.SetCell(i + (maxRange - minRange + 1), column.Key, value);
                    _grid.SetCell(i, column.Key, -1);
                    emptyGroup.Add(i, column.Key);
                    if(emptyGroup.Contains( i + (maxRange - minRange + 1), column.Key))
                    {
                        emptyGroup.Remove(i + (maxRange - minRange + 1),column.Key);
                    }
                }
            }
        }
    }

    public void Fill(BlastGrid2D<int> grid)
    {
        foreach (var group in EmptyGroups)
        {
            var columns = group.GetColumns();
            foreach (var column in columns)
            {
                var rowIds = column.Value.OrderByDescending(x => x);
                foreach (var row in rowIds)
                {
                    var rand = UnityEngine.Random.Range(0, 6);
                    grid.SetCell(row, column.Key,rand);
                }
            }
        }
        EmptyGroups.Clear();
    }

    public void Shuffle(BlastGrid2D<int> grid)
    {
        bool[,] availabilityMap = new bool[grid.RowLenght,grid.ColumnLenght]; 

        var rowIndiceis = Enumerable.Range(0,grid.RowLenght);
        var ColumnIndiceis = Enumerable.Range(0,grid.ColumnLenght);
        var valueCountPairs = GroupByValue(grid);

        foreach (var pair in valueCountPairs)
        {
            var freecell  = EmptyGroups.First().RandomCell();
            
            grid.SetCell(freecell.Row,freecell.Column,pair.Key);
            availabilityMap[freecell.Row,freecell.Column] = true;
            var dirs = GetPossibleDirection(freecell.Row,freecell.Column,availabilityMap).Take(pair.Value-1);

            foreach (var direction in dirs)
            {
                grid.SetCell(direction.Row,direction.Column,pair.Key);
                availabilityMap[direction.Row,direction.Column] = true;
                EmptyGroups.First().Remove(direction.Row,direction.Column);
            }

            for (int i = 0; i < (pair.Value -1) - dirs.Count(); i++)
            {
                var r = EmptyGroups.First().RandomCell();
                grid.SetCell(r.Row,r.Column,pair.Key);
                availabilityMap[r.Row,r.Column] = true;
            }
        }
    }


    private Dictionary<int,int> GroupByValue(BlastGrid2D<int> grid)
    {
        var result = new Dictionary<int,int>();
        var freeCells = new BlastGroup();
        EmptyGroups.Clear();
        EmptyGroups.Add(freeCells);
        grid.TraverseAll((row, column)=>
        {
            freeCells.Add(row,column);
            var value = grid.GetCell(row,column);
            if(!result.ContainsKey(value))
            {
                result.Add(value,0);
            }

            result[value] =  result[value] + 1;
        });

        return result;
    }
    

    private List< BlastGroup.CellPosition> GetPossibleDirection(int row, int column , bool[,] availabilityMap)
    {
        List< BlastGroup.CellPosition> availiblePositions = new List<BlastGroup.CellPosition>();
        
        if (row != availabilityMap.GetLength(0)-1)
        {
           if(!availabilityMap[row + 1,column])
           {
               availiblePositions.Add(new BlastGroup.CellPosition(){ Row = row + 1 , Column = column });
           }
        }
        if (column != availabilityMap.GetLength(1)-1)
        {
           if(!availabilityMap[row,column + 1])
           {
               availiblePositions.Add(new BlastGroup.CellPosition(){ Row = row, Column = column + 1 });
           }
        }
        if (row != 0)
        {
           if(!availabilityMap[row -1,column])
           {
               availiblePositions.Add(new BlastGroup.CellPosition(){ Row = row -1, Column = column });
           }
        }

        if (column != 0)
        {
           if(!availabilityMap[row,column -1])
           {
               availiblePositions.Add(new BlastGroup.CellPosition(){ Row = row, Column = column -1 });
           }
        }

        return availiblePositions;
    }

}
