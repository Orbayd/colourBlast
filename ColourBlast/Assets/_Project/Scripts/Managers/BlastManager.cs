
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlastManager
{
    private BlastGroupConfig _blastConfig;

    private BlastItemFactory _factory;

    public List<BlastGroup> BlastGroups = new List<BlastGroup>();
    public List<BlastGroup> EmptyGroups = new List<BlastGroup>();

    private BlastGridShuffler _shuffler;
    private BlastGridCollapser _collapser;
    private BlastGridFiller _filler;
    private BlastGridGrouper _grouper;

    public BlastManager(BlastGroupConfig blastConfig, BlastItemFactory factory)
    {
        _blastConfig = blastConfig;
        _factory = factory;

        _shuffler = new BlastGridShuffler(EmptyGroups);
        _collapser = new BlastGridCollapser(EmptyGroups);
        _filler = new BlastGridFiller(EmptyGroups, _factory);
        _grouper = new BlastGridGrouper(BlastGroups);
    }
    public BlastGroup Find(int row, int column)
    {
        return BlastGroups.FirstOrDefault(x => x.Contains(row, column));
    }
    public void CreateBlastGrid(AnimatedBlastGrid2D<BlastItem> _grid)
    {
        _grid.TraverseAll((row, column) =>
        {
            var item = _factory.CreateRandom();
            _grid.SetCell(row, column, item);
            item.name = $"GridItem_[{row},{column}]";
        });

    }
     public void CreateBlastableGroups(AnimatedBlastGrid2D<BlastItem> grid)
     {
         _grouper.CreateBlastableGroups(grid);
         SetBlastGroupSprites(grid);

     }
    /*
    public void CreateBlastableGroups(AnimatedBlastGrid2D<BlastItem> _grid)
    {
        BlastGroups.Clear();
        _grid.TraverseAll((row, column) =>
       {
           var blastgroup = Find(row, column);
           if (blastgroup is null)
           {
               //If not create new Group
               var value = _grid.GetCell(row, column);
               blastgroup = new BlastGroup() { Value = value.BlastColour };
               blastgroup.Add(row, column);
               BlastGroups.Add(blastgroup);
               CreateBlastableGroupsInner(row, column, _grid, blastgroup);
           }
       });

        SetBlastGroupSprites(_grid);
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
*/
    private void SetBlastGroupSprites(AnimatedBlastGrid2D<BlastItem> grid)
    {
        foreach (var blastGroup in BlastGroups)
        {
            var positions = blastGroup.GetCellPositions();
            Sprite sprite;
            if (positions.Count <= _blastConfig.A)
            {
                sprite = _blastConfig.Atlast.GetSprite($"{blastGroup.Value}_Default");
            }
            else if (positions.Count <= _blastConfig.B)
            {
                sprite = _blastConfig.Atlast.GetSprite($"{blastGroup.Value}_A");
            }
            else if (positions.Count <= _blastConfig.C)
            {
                sprite = _blastConfig.Atlast.GetSprite($"{blastGroup.Value}_B");
            }
            else
            {
                sprite = _blastConfig.Atlast.GetSprite($"{blastGroup.Value}_C");
            }

            foreach (var position in positions)
            {
                var blastitem = grid.GetCell(position.Row, position.Column);
                blastitem.SetImage(sprite);
            }
        }

    }
    // public void Collapse(AnimatedBlastGrid2D<BlastItem> _grid, int columnLenght, BlastGroup group)
    // {
    //     var colums = group.GetColumns();
    //     foreach (var column in colums)
    //     {
    //         var emptyGroup = new BlastGroup();
    //         EmptyGroups.Add(emptyGroup);
    //         Debug.Log("Collapsing");
    //         Debug.Log($"Column[{column.Key}] Rows: " + string.Join(",", column.Value.Select(x => x.ToString())));
    //         var rowIds = column.Value.OrderByDescending(x => x);

    //         var maxRange = rowIds.First();
    //         var minRange = rowIds.Last();
    //         for (int i = maxRange; i >= minRange; i--)
    //         {
    //             if (_grid.GetCell(i, column.Key) != null)
    //             {
    //                 GameObject.Destroy(_grid.GetCell(i, column.Key).gameObject);
    //             }
    //             _grid.SetCell(i, column.Key, null);
    //             emptyGroup.Add(i, column.Key);
    //         }

    //         if (minRange > 0)
    //         {
    //             for (int i = minRange - 1; i >= 0; i--)
    //             {
    //                 if (_grid.GetCell(i + (maxRange - minRange + 1), column.Key) != null)
    //                 {
    //                     GameObject.Destroy(_grid.GetCell(i + (maxRange - minRange + 1), column.Key).gameObject);
    //                 }
    //                 var value = _grid.GetCell(i, column.Key);
    //                 if (value != null)
    //                 {
    //                     _grid.SetCell(i + (maxRange - minRange + 1), column.Key, value);
    //                 }
    //                 _grid.SetCell(i, column.Key, null);
    //                 emptyGroup.Add(i, column.Key);
    //                 if (emptyGroup.Contains(i + (maxRange - minRange + 1), column.Key))
    //                 {
    //                     emptyGroup.Remove(i + (maxRange - minRange + 1), column.Key);
    //                 }
    //             }
    //         }
    //     }
    // }

    public void Collapse(AnimatedBlastGrid2D<BlastItem> grid, int columnLenght, BlastGroup group)
    {
        _collapser.Collapse(grid,columnLenght,group);
    }
    public void Fill(AnimatedBlastGrid2D<BlastItem> grid)
    {
        _filler.Fill(grid);
    }
    // public void Fill(AnimatedBlastGrid2D<BlastItem> grid)
    // {
    //     foreach (var group in EmptyGroups)
    //     {
    //         var columns = group.GetColumns();
    //         foreach (var column in columns)
    //         {
    //             var rowIds = column.Value.OrderByDescending(x => x);
    //             foreach (var row in rowIds)
    //             {
    //                 var blastItem = _factory.CreateRandom();
    //                 grid.SetCell(row, column.Key, blastItem);

    //             }
    //         }
    //     }
    //     EmptyGroups.Clear();
    // }

    public void Shuffle(AnimatedBlastGrid2D<BlastItem> grid)
    {
        _shuffler.Shuffle(grid);
    }
    /*
    public void Shuffle(AnimatedBlastGrid2D<BlastItem> grid)
    {
        bool[,] availabilityMap = new bool[grid.RowLenght, grid.ColumnLenght];

        var rowIndiceis = Enumerable.Range(0, grid.RowLenght);
        var ColumnIndiceis = Enumerable.Range(0, grid.ColumnLenght);
        var valueCountPairs = GroupByValue(grid);

        foreach (var pair in valueCountPairs)
        {
            var freecell = EmptyGroups.First().RandomCell();

            var blastItem = pair.Value.First();
            grid.SetCell(freecell.Row, freecell.Column, blastItem );
            pair.Value.Remove(blastItem);

            availabilityMap[freecell.Row, freecell.Column] = true;
            var dirs = GetPossibleDirection(freecell.Row, freecell.Column, availabilityMap).Take(pair.Value.Count);
            foreach (var direction in dirs)
            {
                blastItem = pair.Value.First();
                grid.SetCell(direction.Row, direction.Column, blastItem);
                pair.Value.Remove(blastItem);

                availabilityMap[direction.Row, direction.Column] = true;
                EmptyGroups.First().Remove(direction.Row, direction.Column);
            }

            for (int i = 0; i < pair.Value.Count; i++)
            {
                var r = EmptyGroups.First().RandomCell();
                
                blastItem = pair.Value.First();
                grid.SetCell(r.Row, r.Column, blastItem);
                pair.Value.Remove(blastItem);
                availabilityMap[r.Row, r.Column] = true;
            }
        }
    }


    private Dictionary<BlastColour, List<BlastItem>> GroupByValue(AnimatedBlastGrid2D<BlastItem> grid)
    {
        var result = new Dictionary<BlastColour, List<BlastItem>>();
        var freeCells = new BlastGroup();
        EmptyGroups.Clear();
        EmptyGroups.Add(freeCells);
        grid.TraverseAll((row, column) =>
        {
            freeCells.Add(row, column);
            var value = grid.GetCell(row, column);
            if (!result.ContainsKey(value.BlastColour))
            {
                result.Add(value.BlastColour, new List<BlastItem>());
            }

            result[value.BlastColour].Add(value);
        });

        return result;
    }


    private List<BlastGroup.CellPosition> GetPossibleDirection(int row, int column, bool[,] availabilityMap)
    {
        List<BlastGroup.CellPosition> availiblePositions = new List<BlastGroup.CellPosition>();

        if (row != availabilityMap.GetLength(0) - 1)
        {
            if (!availabilityMap[row + 1, column])
            {
                availiblePositions.Add(new BlastGroup.CellPosition() { Row = row + 1, Column = column });
            }
        }
        if (column != availabilityMap.GetLength(1) - 1)
        {
            if (!availabilityMap[row, column + 1])
            {
                availiblePositions.Add(new BlastGroup.CellPosition() { Row = row, Column = column + 1 });
            }
        }
        if (row != 0)
        {
            if (!availabilityMap[row - 1, column])
            {
                availiblePositions.Add(new BlastGroup.CellPosition() { Row = row - 1, Column = column });
            }
        }

        if (column != 0)
        {
            if (!availabilityMap[row, column - 1])
            {
                availiblePositions.Add(new BlastGroup.CellPosition() { Row = row, Column = column - 1 });
            }
        }

        return availiblePositions;
    }
    */
    public bool HasBlastable()
    {
        return BlastGroups.Any(x => x.IsBlastable);
    }


}
