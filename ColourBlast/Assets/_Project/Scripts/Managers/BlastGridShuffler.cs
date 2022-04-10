
using System.Collections.Generic;
using System.Linq;

public class BlastGridShuffler
{
    private List<BlastGroup> _emptyGroups;
    public BlastGridShuffler(List<BlastGroup> emptyGroups)
    {
        _emptyGroups = emptyGroups;
    }
    public void Shuffle(AnimatedBlastGrid2D<BlastItem> grid)
    {
        bool[,] availabilityMap = new bool[grid.RowLenght, grid.ColumnLenght];

        var rowIndiceis = Enumerable.Range(0, grid.RowLenght);
        var ColumnIndiceis = Enumerable.Range(0, grid.ColumnLenght);
        var valueCountPairs = GroupByValue(grid ,_emptyGroups);

        foreach (var pair in valueCountPairs)
        {
            var freecell = _emptyGroups.First().RandomCell();

            var blastItem = pair.Value.First();
            grid.SetCell(freecell.Row, freecell.Column, blastItem);
            pair.Value.Remove(blastItem);

            availabilityMap[freecell.Row, freecell.Column] = true;
            var dirs = GetPossibleDirection(freecell.Row, freecell.Column, availabilityMap,_emptyGroups).Take(pair.Value.Count);
            foreach (var direction in dirs)
            {
                blastItem = pair.Value.First();
                grid.SetCell(direction.Row, direction.Column, blastItem);
                pair.Value.Remove(blastItem);

                availabilityMap[direction.Row, direction.Column] = true;
                _emptyGroups.First().Remove(direction.Row, direction.Column);
            }

            for (int i = 0; i < pair.Value.Count; i++)
            {
                var r = _emptyGroups.First().RandomCell();

                blastItem = pair.Value.First();
                grid.SetCell(r.Row, r.Column, blastItem);
                pair.Value.Remove(blastItem);
                availabilityMap[r.Row, r.Column] = true;
            }
        }
    }


    private Dictionary<BlastColour, List<BlastItem>> GroupByValue(AnimatedBlastGrid2D<BlastItem> grid,List<BlastGroup> emptyGroups)
    {
        var result = new Dictionary<BlastColour, List<BlastItem>>();
        var freeCells = new BlastGroup();
        emptyGroups.Clear();
        emptyGroups.Add(freeCells);
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


    private List<BlastGroup.CellPosition> GetPossibleDirection(int row, int column, bool[,] availabilityMap ,List<BlastGroup> emptyGroups)
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


}