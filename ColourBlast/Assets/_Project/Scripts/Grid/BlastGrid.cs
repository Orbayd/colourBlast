using System;
using System.Linq;

public class BlastGrid2D<T> : IBlastGrid2D<T>
{

    private T[,] _cells;

    public float CellSize { get; private set; }

    public int ColumnLenght { get; private set; }

    public int RowLenght{ get; private set; }


    public BlastGrid2D(BlastGridConfig config)
    {
        _cells = new T[config.ColumnLenght, config.RowLenght];
        ColumnLenght = config.ColumnLenght;
        RowLenght = config.RowLenght;
        CellSize = config.CellSize;
    }

    public void TraverseAll(Action<int, int> callback)
    {
        for (int irow = 0; irow < _cells.GetLength(0); irow++)
        {
            for (int icolumn = 0; icolumn < _cells.GetLength(1); icolumn++)
            {
                callback?.Invoke(irow,icolumn);
            }
        }
    }

    public void SetCell(int row, int column, T data)
    {
        _cells[row, column] = data;

    }

    public T GetCell(int row, int column)
    {
        return _cells[row, column];
    }

    public T[] GetColumnItems(int columnId)
    {
        return Enumerable.Range(0, ColumnLenght)
            .Select(x => _cells[x, columnId])
            .ToArray();
    }

    public CellPosition[] GetColumn(int columnId)
    {
        return Enumerable.Range(0, ColumnLenght)
            .Select(x => new CellPosition(){ Row = x, Column = columnId})
            .ToArray();
    }

     public CellPosition[] GetRow(int rowId)
    {
        return Enumerable.Range(0, RowLenght)
            .Select(x => new CellPosition(){ Row = rowId, Column = x})
            .ToArray();
    }

    public T[] GetRowItems(int rowId)
    {
        return Enumerable.Range(0, RowLenght)
            .Select(x => _cells[rowId, x])
            .ToArray();
    }
}
