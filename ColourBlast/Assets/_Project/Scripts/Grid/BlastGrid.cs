using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlastGrid2D<T> : IBlastGrid2D<T>
{

    private T[,] _cells;

    public float CellSize { get; private set; }

    public int RowLenght { get; private set; }

    public int ColumnLenght{ get; private set; }

    private Vector2 _origin;

    public BlastGrid2D(BlastGridConfig config)
    {
        _cells = new T[config.RowLenght, config.ColumnLenght];
        RowLenght = config.RowLenght;
        ColumnLenght = config.ColumnLenght;
        CellSize = config.CellSize;
    }

    public void SetPosition(Vector2 position)
    {
        _origin = position;
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

    public Vector2 GridToWorldPosition(int row, int column)
    {
        return new Vector2(column, - row) * CellSize - _origin;
    }

    public Vector2Int WorldToGridPosition(Vector2 worldPosition)
    {
        var x = Mathf.FloorToInt(-(worldPosition + _origin).y / CellSize);
        var y = Mathf.FloorToInt((worldPosition + _origin).x / CellSize);

        return new Vector2Int(x, y);
    }

    public void SetCell(int row, int column, T data)
    {
        _cells[row, column] = data;

    }

    public void SetCell(Vector2 position, T data)
    {
        var cellPosition = WorldToGridPosition(position);
        _cells[cellPosition.x, cellPosition.y] = data;
    }

    public T GetCell(int row, int column)
    {
        return _cells[row, column];
    }

    public T GetCell(Vector2 position)
    {
        var cellPosition = WorldToGridPosition(position);
        return _cells[cellPosition.x, cellPosition.y];
    }


    public T[] GetColumn(int columnId)
    {
        return Enumerable.Range(0, RowLenght)
            .Select(x => _cells[x, columnId])
            .ToArray();
    }

    public T[] GetRow(int rowId)
    {
        return Enumerable.Range(0, ColumnLenght)
            .Select(x => _cells[rowId, x])
            .ToArray();
    }
}
