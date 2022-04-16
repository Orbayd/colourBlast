using System;
using UnityEngine;

namespace ColourBlast.Grid2D
{
    public interface IBlastGrid2D<T>
    {
        void TraverseAll(Action<CellPosition> callback);
        void SetCell(int row, int column, T data);
        T GetCell(int row, int column);
        T[] GetColumnItems(int columnId);
        T[] GetRowItems(int rowId);

    }
}
