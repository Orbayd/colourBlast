
using System;
using UnityEngine;

public class AnimatedBlastGrid2D<T> : IBlastGrid2D<T> where T : MonoBehaviour
{
    private BlastGrid2D<T> _grid;

    public float CellSize => _grid.CellSize;

    public int RowLenght => _grid.RowLenght;

    public int ColumnLenght => _grid.ColumnLenght;

    public AnimatedBlastGrid2D(BlastGrid2D<T> grid)
    {
        _grid = grid;
    }

    public void SetPosition(Vector2 position)
    {
        _grid.SetPosition(position);
    }

    public void TraverseAll(Action<int, int> callback)
    {
        _grid.TraverseAll((x, y) =>
        {
            callback?.Invoke(x, y);
        });
    }

    public Vector2 GridToWorldPosition(int row, int column)
    {
        return _grid.GridToWorldPosition(row, column);
    }

    public Vector2Int WorldToGridPosition(Vector2 worldPosition)
    {
        return _grid.WorldToGridPosition(worldPosition);
    }

    public void SetCell(int row, int column, T data)
    {
        _grid.SetCell(row, column, data);

        if (data != null)
        {
            var position = _grid.GridToWorldPosition(row, column);
            MoveTo(position, data);
        }
    }

    public void SetCell(Vector2 position, T data)
    {
        _grid.SetCell(position, data);
        if (data != null)
        {
            MoveTo(position, data);
        }
    }

    public T GetCell(int row, int column)
    {
        return _grid.GetCell(row, column);
    }

    public T GetCell(Vector2 position)
    {
        return _grid.GetCell(position);
    }


    public T[] GetColumn(int columnId)
    {
        return _grid.GetColumn(columnId);
    }

    public T[] GetRow(int rowId)
    {
        return _grid.GetRow(rowId);
    }

    private void MoveTo(Vector2 position, T data)
    {
        data.transform.position = position;
    }

}