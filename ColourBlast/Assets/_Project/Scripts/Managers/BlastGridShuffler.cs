
using System.Collections.Generic;
using System.Linq;

public class BlastGridShuffler
{
    public List<CellPosition> _emptyPositions;

    public BlastGridShuffler()
    {
        
    }
    public void Shuffle(AnimatedBlastGrid2D<BlastItem> grid)
    {
        bool[,] availabilityMap = new bool[grid.RowLenght, grid.ColumnLenght];
        _emptyPositions = grid.GetEmptyCells().ToList();
        var valueCountPairs = GroupByValue(grid);
        foreach (var pair in valueCountPairs)
        {
            //var freecell = _emptyGroups.First().RandomCell();
            var freecell = RandomCell();

            var blastItem = pair.Value.First();
            grid.SetCell(freecell.Row, freecell.Column, blastItem);
            pair.Value.Remove(blastItem);

            availabilityMap[freecell.Row, freecell.Column] = true;
            var dirs = GetPossibleDirection(freecell.Row, freecell.Column, availabilityMap).Take(pair.Value.Count);
            foreach (var direction in dirs)
            {
                blastItem = pair.Value.First();
                grid.SetCell(direction.Row, direction.Column, blastItem);
                pair.Value.Remove(blastItem);

                availabilityMap[direction.Row, direction.Column] = true;
                _emptyPositions.RemoveAll(x => x.Row == direction.Row && x.Column == direction.Column);
            }

            for (int i = 0; i < pair.Value.Count; i++)
            {
                var r = RandomCell();

                blastItem = pair.Value.First();
                grid.SetCell(r.Row, r.Column, blastItem);
                pair.Value.Remove(blastItem);
                availabilityMap[r.Row, r.Column] = true;
            }
        }

        _emptyPositions.Clear();
    }


    private Dictionary<BlastColour, List<BlastItem>> GroupByValue(AnimatedBlastGrid2D<BlastItem> grid)
    {
        var result = new Dictionary<BlastColour, List<BlastItem>>();
        
        grid.TraverseAll((row, column) =>
        {
            var value = grid.GetCell(row, column);
            if (!result.ContainsKey(value.BlastColour))
            {
                result.Add(value.BlastColour, new List<BlastItem>());
            }

            result[value.BlastColour].Add(value);
        });

        return result;
    }


    private List<CellPosition> GetPossibleDirection(int row, int column, bool[,] availabilityMap)
    {
        List<CellPosition> availiblePositions = new List<CellPosition>();

        if (row != availabilityMap.GetLength(0) - 1)
        {
            if (!availabilityMap[row + 1, column])
            {
                availiblePositions.Add(new CellPosition() { Row = row + 1, Column = column });
            }
        }
        if (column != availabilityMap.GetLength(1) - 1)
        {
            if (!availabilityMap[row, column + 1])
            {
                availiblePositions.Add(new CellPosition() { Row = row, Column = column + 1 });
            }
        }
        if (row != 0)
        {
            if (!availabilityMap[row - 1, column])
            {
                availiblePositions.Add(new CellPosition() { Row = row - 1, Column = column });
            }
        }

        if (column != 0)
        {
            if (!availabilityMap[row, column - 1])
            {
                availiblePositions.Add(new CellPosition() { Row = row, Column = column - 1 });
            }
        }

        return availiblePositions;
    }


    public CellPosition RandomCell()
    {
        var rand = UnityEngine.Random.Range(0, _emptyPositions.Count);
        var randomCell = _emptyPositions.ElementAt(rand);
        _emptyPositions.Remove(randomCell);
        return randomCell;
    }


}