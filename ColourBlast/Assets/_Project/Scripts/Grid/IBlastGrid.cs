using System;
using UnityEngine;

public interface IBlastGrid2D<T>
{
    void TraverseAll(Action<int, int> callback);  
    void SetCell(int row, int column, T data);
    T GetCell(int row, int column);
    T[] GetColumnItems(int columnId);
    T[] GetRowItems(int rowId);
    
}
