using System;
using UnityEngine;

public interface IBlastGrid2D<T>
{
    void SetPosition(Vector2 position);
    void TraverseAll(Action<int, int> callback);  
    Vector2 GridToWorldPosition(int row, int column);
    Vector2Int WorldToGridPosition(Vector2 worldPosition);
    void SetCell(int row, int column, T data);
    void SetCell(Vector2 position, T data); 
    T GetCell(int row, int column);
    T GetCell(Vector2 position);   
    T[] GetColumn(int columnId);
    T[] GetRow(int rowId);
    
}
