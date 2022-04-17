using System.Collections.Generic;
using ColourBlast.Enums;
using ColourBlast.Grid2D;
using UnityEngine;

namespace ColourBlast.Helpers
{
    public static class CameraExtensions
    {
        private static void InitCamera(this Camera camera, int RowLenght, int ColumnLenght, float CellSize)
        {
            var verticalExtend = camera.orthographicSize * 2;
            var horizontalExtend = verticalExtend * Screen.width / Screen.height;

            var gridVerticalExtend = (float)ColumnLenght * CellSize;
            var gridHorizontalExtend = (float)RowLenght * CellSize;

            if (gridHorizontalExtend > horizontalExtend)
            {
                Camera.main.orthographicSize *= gridHorizontalExtend / horizontalExtend;
            }
            if (gridVerticalExtend > verticalExtend)
            {
                Camera.main.orthographicSize *= gridVerticalExtend / verticalExtend;
            }

            verticalExtend = camera.orthographicSize;
            horizontalExtend = Screen.width / Screen.height * verticalExtend;

            camera.transform.position = new Vector3(gridHorizontalExtend / 2f - CellSize / 2f,
                                                          -verticalExtend + CellSize / 2f, -10);

        }
    }

    public static class Grid2DExtensions
    {
        public static Dictionary<BlastColour, List<BlastItem>> GroupByValue(this AnimatedBlastGrid2D<BlastItem> grid)
        {
            var result = new Dictionary<BlastColour, List<BlastItem>>();

            grid.TraverseAll((position) =>
            {
                var value = grid.GetCell(position);
                if (!result.ContainsKey(value.BlastColour))
                {
                    result.Add(value.BlastColour, new List<BlastItem>());
                }
                result[value.BlastColour].Add(value);
            });

            return result;
        }

        public static Dictionary<int, List<int>> GroupByColumns(this AnimatedBlastGrid2D<BlastItem> grid, IEnumerable<CellPosition> source)
        {
            Dictionary<int, List<int>> colums = new Dictionary<int, List<int>>();
            foreach (var cellPosition in source)
            {
                if (!colums.ContainsKey(cellPosition.Column))
                {
                    colums.Add(cellPosition.Column, new List<int>());
                }

                colums[cellPosition.Column].Add(cellPosition.Row);
            }

            return colums;
        }

        public static void Clear(this AnimatedBlastGrid2D<BlastItem> grid)
        {
            grid.TraverseAll((position) =>
            {
                grid.SetEmpty(position);
            });
        }

        public static CellPosition[] GetEmptyCells(this AnimatedBlastGrid2D<BlastItem> grid)
        {
            List<CellPosition> emptyCells = new List<CellPosition>();
            grid.TraverseAll((position) =>
            {
                if (grid.GetCell(position.Row, position.Column) == default)
                {
                    emptyCells.Add(new CellPosition(position.Row, position.Column));
                }
            });
            return emptyCells.ToArray();
        }

         public static CellPosition[] GetEmptyCells<T>(this BlastGrid2D<T> grid, T defaultValue)
        {
            List<CellPosition> emptyCells = new List<CellPosition>();
            grid.TraverseAll((position) =>
            {
                if (grid.GetCell(position.Row, position.Column).Equals(defaultValue))
                {
                    emptyCells.Add(new CellPosition(position.Row, position.Column));
                }
            });
            return emptyCells.ToArray();
        }

    }
}