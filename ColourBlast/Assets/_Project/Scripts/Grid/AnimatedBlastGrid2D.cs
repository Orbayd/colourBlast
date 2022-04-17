
using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace ColourBlast.Grid2D
{
    public class AnimatedBlastGrid2D<T> : IBlastGrid2D<T> where T : GridItem
    {
        private BlastGrid2D<T> _grid;
        public int RowLenght => _grid.RowLenght;

        public int ColumnLenght => _grid.ColumnLenght;

        public Transform Transform { get; private set; }

        public GridLayout2D GridLayout { get; }

        public event Action AllAnimationsCompleted;

        public AnimatedBlastGrid2D(BlastGrid2D<T> grid, GridLayout2D layout)
        {
            _grid = grid;
            Transform = new GameObject("grid").transform;
            GridLayout = layout;
        }

        public void TraverseAll(Action<CellPosition> callback)
        {
            _grid.TraverseAll((positon) =>
            {
                callback?.Invoke(positon);
            });
        }
        public Vector2 GridToWorldPosition(int row, int column)
        {
            return GridLayout.GetGridPosition(row, column);
        }
        public Vector2 GridToWorldPosition(CellPosition position)
        {
            return GridToWorldPosition(position.Row,position.Column);
        }

        public void SetCell(int row, int column, T data)
        {
            data.Position = new CellPosition(row, column);
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
         public void SetEmpty(CellPosition pos)
        {
            SetEmpty(pos.Row, pos.Column);
        }


        public T GetCell(int row, int column)
        {
            return _grid.GetCell(row, column);
        }

        public T GetCell(CellPosition position)
        {
            return _grid.GetCell(position.Row, position.Column);
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
            var columns = _grid.GetColumn(columnId);
            var column = columns.Where(x => _grid.GetCell(x.Row, x.Column) != null).Last();
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
        HashSet<T> _animations = new HashSet<T>();
        private void MoveTo(Vector2 position, T data)
        {
            _animations.Add(data);
            data.transform.DOMove(position, 0.25f, false).SetEase(Ease.Linear)
            .OnComplete(()=>
            {
                _animations.Remove(data);
                if(_animations.Count == 0)
                {
                    AllAnimationsCompleted?.Invoke();
                }
            }).OnKill(()=>
            {
                _animations.Remove(data);
                if(_animations.Count == 0)
                {
                    AllAnimationsCompleted?.Invoke();
                }   
            });

        }
    }
}