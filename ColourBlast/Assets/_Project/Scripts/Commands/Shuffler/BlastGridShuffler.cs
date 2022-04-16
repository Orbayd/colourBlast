
using System.Collections.Generic;
using System.Linq;
using ColourBlast.Grid2D;
using ColourBlast.Helpers;
using UnityEngine;

public interface IShuffleCommand
{
    void Shuffle(AnimatedBlastGrid2D<BlastItem> grid);
}
public class BlastGridShuffler : IShuffleCommand
{
    public List<CellPosition> _emptyPositions;

    public void Shuffle(AnimatedBlastGrid2D<BlastItem> grid)
    {
        bool[,] availabilityMap = new bool[grid.RowLenght, grid.ColumnLenght];
        var valueCountPairs = grid.GroupByValue();       
        grid.Clear();
        _emptyPositions = grid.GetEmptyCells().ToList();
         
        foreach (var pair in valueCountPairs)
        {
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

    private List<CellPosition> GetPossibleDirection(int row, int column, bool[,] availabilityMap)
    {
        List<CellPosition> availiblePositions = new List<CellPosition>();

        var position = new CellPosition(row, column);
        
        if (row < availabilityMap.GetLength(0) - 1)
        {
            if (!availabilityMap[row + 1, column])
            {
                availiblePositions.Add(position.Right);
            }
        }
        if (column < availabilityMap.GetLength(1) - 1)
        {
            if (!availabilityMap[row, column + 1])
            {
                availiblePositions.Add(position.Down);
            }
        }
        if (row > 0)
        {
            if (!availabilityMap[row - 1, column])
            {
                availiblePositions.Add(position.Left);
            }
        }

        if (column > 0)
        {
            if (!availabilityMap[row, column - 1])
            {
                availiblePositions.Add(position.Up);
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