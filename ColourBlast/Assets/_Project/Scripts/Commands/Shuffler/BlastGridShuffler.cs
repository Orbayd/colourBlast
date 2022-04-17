
using System.Collections.Generic;
using System.Linq;
using ColourBlast.Grid2D;
using ColourBlast.Helpers;
using UnityEngine;

public class BlastGridShuffler : IShuffleCommand
{
    public List<CellPosition> _emptyPositions;

    public void Shuffle(AnimatedBlastGrid2D<BlastItem> grid)
    {
        // bool[,] availabilityMap = new bool[grid.RowLenght, grid.ColumnLenght];
        BlastGrid2D<bool> availabilityMap = new BlastGrid2D<bool>(grid.RowLenght, grid.ColumnLenght);
        var valueCountPairs = grid.GroupByValue();
        Debug.Assert(valueCountPairs.SelectMany(x => x.Value).Count() == grid.RowLenght * grid.ColumnLenght);

        grid.Clear();
        foreach (var pair in valueCountPairs)
        {
            var targetCell = RandomCell(availabilityMap);
            SetTargetPosition(grid, availabilityMap, pair.Value, targetCell);
            var dirs = GetPossibleDirection(targetCell, availabilityMap).Take(pair.Value.Count);
            if (dirs.Any())
            {
                foreach (var direction in dirs)
                {      
                    SetTargetPosition(grid, availabilityMap, pair.Value, direction);
                }
            }
            // if(pair.Value.Count> 0)
            // {
            //     Debug.Log("Something is Wrong");
            // }

            for (int i = pair.Value.Count; i > 0 ; i--)
            {
                SetTargetPosition(grid, availabilityMap, pair.Value, RandomCell(availabilityMap));
            }
            // if(pair.Value.Count> 0)
            // {
            //     Debug.Log("Something is Wrong");
            // }
        }
        Debug.Assert(availabilityMap.GetEmptyCells(false).Length == 0, $"");
    }

    private void SetTargetPosition(AnimatedBlastGrid2D<BlastItem> grid, BlastGrid2D<bool> availabilityMap, List<BlastItem> pair, CellPosition targetPosition)
    {
        grid.SetCell(targetPosition.Row, targetPosition.Column, pair.First());
        pair.Remove(pair.First());
        Debug.Assert(availabilityMap.GetCell(targetPosition) == false, $"[{targetPosition.Row},{targetPosition.Column} is not empty!]");
        availabilityMap.SetCell(targetPosition, true); ;
    }

    private List<CellPosition> GetPossibleDirection(CellPosition position, BlastGrid2D<bool> availabilityMap)
    {
        List<CellPosition> availiblePositions = new List<CellPosition>();

        if (position.Row < availabilityMap.RowLenght - 1)
        {
            if (!availabilityMap.GetCell(position.Right))
            {
                availiblePositions.Add(position.Right);
            }
        }
        if (position.Column < availabilityMap.ColumnLenght - 1)
        {
            if (!availabilityMap.GetCell(position.Down))
            {
                availiblePositions.Add(position.Down);
            }
        }
        if (position.Row > 0)
        {
            if (!availabilityMap.GetCell(position.Left))
            {
                availiblePositions.Add(position.Left);
            }
        }

        if (position.Column > 0)
        {
            if (!availabilityMap.GetCell(position.Up))
            {
                availiblePositions.Add(position.Up);
            }
        }

        return availiblePositions;
    }

    public CellPosition RandomCell(BlastGrid2D<bool> availabilityMap)
    {
        var emptyPositions = availabilityMap.GetEmptyCells(false);
        var rand = UnityEngine.Random.Range(0, emptyPositions.Length);
        var randomCell = emptyPositions.ElementAt(rand);
        return randomCell;
    }

    public CellPosition RandomCell()
    {
        var rand = UnityEngine.Random.Range(0, _emptyPositions.Count);
        var randomCell = _emptyPositions.ElementAt(rand);
        _emptyPositions.Remove(randomCell);
        return randomCell;
    }


}