using System;
using System.Linq;
using UnityEngine;

namespace ColourBlast.Grid2D
{
    public class BlastGrid2D<T> : IBlastGrid2D<T>
    {
        private T[,] _cells;
        public int ColumnLenght { get; private set; }
        public int RowLenght { get; private set; }

        public BlastGrid2D(BlastGridConfig config)
        {
            _cells = new T[config.RowLenght, config.ColumnLenght];
            ColumnLenght = config.ColumnLenght;
            RowLenght = config.RowLenght;
        }
        public BlastGrid2D(int rowLenght, int columnLenght)
        {
            _cells = new T[rowLenght, columnLenght];
            ColumnLenght = columnLenght;
            RowLenght = rowLenght;
        }

        public void TraverseAll(Action<CellPosition> callback)
        {
            for (int irow = 0; irow < _cells.GetLength(0); irow++)
            {
                for (int icolumn = 0; icolumn < _cells.GetLength(1); icolumn++)
                {
                    callback?.Invoke(new CellPosition(irow, icolumn));
                }
            }
        }

        public void SetCell(int row, int column, T data)
        {
            _cells[row, column] = data;
        }
        public void SetCell(CellPosition position, T data)
        {
            SetCell(position.Row,position.Column,data);
        }

        public T GetCell(int row, int column)
        {
            return _cells[row, column];
        }

        public T GetCell(CellPosition position)
        {
            return GetCell(position.Row,position.Column);
        }

        public T[] GetColumnItems(int columnId)
        {
            return Enumerable.Range(0, RowLenght)
                .Select(x => _cells[x, columnId])
                .ToArray();
        }

        public CellPosition[] GetColumn(int columnId)
        {
            return Enumerable.Range(0, RowLenght)
                .Select(x => new CellPosition(x,columnId))
                .ToArray();
        }

        public CellPosition[] GetRow(int rowId)
        {
            return Enumerable.Range(0, ColumnLenght)
                .Select(x => new CellPosition(rowId,x))
                .ToArray();
        }

        public T[] GetRowItems(int rowId)
        {
            return Enumerable.Range(0, ColumnLenght)
                .Select(x => _cells[rowId, x])
                .ToArray();
        }
    }
}