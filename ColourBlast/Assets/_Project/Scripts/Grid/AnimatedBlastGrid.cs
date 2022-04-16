
using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public  class AnimatedBlastGrid2D<T> : IBlastGrid2D<T> where T : MonoBehaviour
{
    private BlastGrid2D<T> _grid;

    public float CellSize => _grid.CellSize;

    public int RowLenght => _grid.RowLenght;

    public int ColumnLenght => _grid.ColumnLenght;

    public Transform Transform { get; private set; }

    private Vector2 _origin;

    public GridLayout2D GridLayout {get;}

    public AnimatedBlastGrid2D(BlastGrid2D<T> grid, GridLayout2D layout)
    {
        _grid = grid;
        Transform = new GameObject("grid").transform;
        GridLayout = layout;
    }

    public void SetPosition(Vector2 position)
    {
        _origin = position;
        Transform.position = position;
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
        return GridLayout.GetGridPosition(row, column);
    }

    public Vector2Int WorldToGridPosition(Vector2 worldPosition)
    {
        //  var x = Mathf.FloorToInt(-(worldPosition + _origin).y / CellSize);
        //  var y = Mathf.FloorToInt((worldPosition + _origin).x / CellSize);

        // return new Vector2Int(x, y);
        return GridLayout.WorldToGridPosition(worldPosition);
    }

    public void SetCell(int row, int column, T data)
    {
        _grid.SetCell(row, column, data);

        if (data != null)
        {
            data.transform.SetParent(Transform);
            var position = GridToWorldPosition(row, column);
            MoveTo(position, data);
        }
    }
    public void SetEmpty(int row, int column)
    {
        _grid.SetCell(row, column, default);
    }

    public void SetCell(Vector2 position, T data)
    {
        var cellPosition = WorldToGridPosition(position);
        _grid.SetCell(cellPosition.x, cellPosition.y, data);
        if (data != null)
        {
            MoveTo(position, data);
        }
    }

    public T GetCell(int row, int column)
    {
        return _grid.GetCell(row, column);
    }

    public T GetCell(CellPosition position)
    {
        return _grid.GetCell(position.Row, position.Column);
    }

    public T GetCell(Vector2 position)
    {
        var cellPosition = WorldToGridPosition(position);
        return _grid.GetCell(cellPosition.x, cellPosition.y);
    }


    public T[] GetColumnItems(int columnId)
    {
        return _grid.GetColumnItems(columnId);
    }

    public T[] GetRowItems(int rowId)
    {
        return _grid.GetRowItems(rowId);
    }

    public T RemoveLastItemInColumn(int columnId)
    {
        var column = _grid.GetColumn(columnId).Where(x => _grid.GetCell(x.Row, x.Column) != null).Last();
        var item = _grid.GetCell(column.Row, column.Column);
        _grid.SetCell(column.Row, column.Column, null);
        return item;
    }
    public T RemoveFirstItemInColumn(int columnId)
    {
        var column = _grid.GetColumn(columnId).Where(x => _grid.GetCell(x.Row, x.Column)).First();
        var item = _grid.GetCell(column.Row, column.Column);
        _grid.SetCell(column.Row, column.Column, null);
        return item;
    }

    public CellPosition[] GetEmptyCells()
    {
        List<CellPosition> emptyCells = new List<CellPosition>();
        _grid.TraverseAll((x, y) =>
        {
            if (_grid.GetCell(x, y) == default)
            {
                emptyCells.Add(new CellPosition() { Row = x, Column = y });
            }
        });

        return emptyCells.ToArray();
    }

    public Dictionary<int, List<int>> GroupByColumns(IEnumerable<CellPosition> source)
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

    private void MoveTo(Vector2 position, T data)
    {
        data.transform.DOMove(position, 0.25f, false).SetEase(Ease.Linear);
    }
}