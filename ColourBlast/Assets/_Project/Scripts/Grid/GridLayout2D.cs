using UnityEngine;

public class GridLayout2D
{
    public GridAnchorPosition AnchorPosition;

    public bool IsFlexible {get;set;}= false;
    public bool IsFixedSize {get;set;} = false;
    public int RowLenght {get;}
    public int ColumnLenght {get;}
    public float CellSize;
    public float CellHeight {get; private set;}
    public float CellWidth {get; private set;}

    public Vector2 Offset;


    public GridLayout2D(int rowLenght, int columnLenght, float cellSize ,bool IsFixedSize, bool IsFlexible)
    {
        RowLenght = rowLenght;
        ColumnLenght = columnLenght;
        CellSize = cellSize;
        
        var camera = Camera.main;
        Vector3 topLeft = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane));
        Vector3 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        Vector3 bottomleft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        Vector3 bottomRight = camera.ViewportToWorldPoint(new Vector3(1, 0, camera.nearClipPlane));
        
        CellWidth = Vector3.Distance(topLeft, topRight) / ColumnLenght;
        CellHeight = Vector3.Distance(bottomRight, topRight) / RowLenght;
        if (IsFixedSize)
        {
            CellHeight = Mathf.Min(CellHeight, CellSize);
            CellWidth = Mathf.Min(CellWidth, CellSize);
        }

        if (!IsFlexible)
        {
            CellHeight = Mathf.Min(CellHeight, CellWidth);
            CellWidth = Mathf.Min(CellHeight, CellWidth);
        }
    }

    public Vector3 GetGridPosition(int row, int column)
    {
        return GetGridPosition(AnchorPosition, row, column, new Vector2(CellWidth, CellHeight));
    }
    public Vector2Int WorldToGridPosition(Vector2 worldPosition)
    {
        return WorldToGridPosition(AnchorPosition,worldPosition,new Vector2(CellWidth, CellHeight));
    }

    Vector3 GetGridPosition(GridAnchorPosition anchorPosition, int row, int column,  Vector2 cellSize)
    {
        var camera = Camera.main;
        Vector3 gridposition = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane));
        var bounds = GetGridBounds(RowLenght, ColumnLenght, cellSize);

        Vector3 topLeft = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane));
        Vector3 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        Vector3 bottomleft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        Vector3 bottomRight = camera.ViewportToWorldPoint(new Vector3(1, 0, camera.nearClipPlane));

        switch (anchorPosition)
        {
            case GridAnchorPosition.UpperLeft:

                break;
            case GridAnchorPosition.UpperMiddle:
                gridposition = Vector3.Lerp(gridposition, topRight, 0.5f);
                gridposition -= new Vector3(bounds.x * 0.5f, 0, 0);
                break;
            case GridAnchorPosition.UpperRight:
                gridposition = topRight - new Vector3(bounds.x, 0, 0);
                break;
            case GridAnchorPosition.MiddleLeft:

                gridposition = Vector3.Lerp(topLeft, bottomleft, 0.5f);
                gridposition += new Vector3(0, bounds.y * 0.5f, 0);
                break;
            case GridAnchorPosition.Middle:

                gridposition = Vector3.Lerp(Vector3.Lerp(topLeft, bottomleft, 0.5f), Vector3.Lerp(bottomRight, topRight, 0.5f), 0.5f);
                gridposition += new Vector3(-bounds.x * 0.5f, bounds.y * 0.5f, 0);

                break;
            case GridAnchorPosition.MiddleRight:
                gridposition = Vector3.Lerp(topRight, bottomRight, 0.5f);
                gridposition += new Vector3(-bounds.x, bounds.y * 0.5f, 0);
                break;
            case GridAnchorPosition.BottomLeft:
                gridposition = bottomleft;
                gridposition += new Vector3(0, bounds.y);
                break;
            case GridAnchorPosition.BottomMiddle:
                gridposition = Vector3.Lerp(bottomleft, bottomRight, 0.5f);
                gridposition += new Vector3(-bounds.x * 0.5f, bounds.y);
                break;
            case GridAnchorPosition.BottomRight:
                gridposition = bottomRight - new Vector3(bounds.x, -bounds.y, 0);
                break;

            default: break;
        }
        var position = new Vector3(gridposition.x + column * cellSize.x, gridposition.y - row * cellSize.y, 0) +  new Vector3(Offset.x,Offset.y,0);
        gridposition = position + new Vector3(cellSize.x * 0.5f, -cellSize.y * 0.5f);

        return gridposition;
    }

    //TODO
    Vector2Int WorldToGridPosition(GridAnchorPosition anchorPosition, Vector2 worldPosition , Vector2 cellSize)
    {

        //var x = Mathf.FloorToInt(-(worldPosition + _origin).y / CellSize);
        // var y = Mathf.FloorToInt((worldPosition + _origin).x / CellSize);
        Vector2Int gridPosition = new Vector2Int(-1,-1);
        var camera = Camera.main;
        Vector3 gridposition = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane));
        var bounds = GetGridBounds(RowLenght, ColumnLenght, cellSize);

        Vector3 topLeft = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane));
        Vector3 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        Vector3 bottomleft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        Vector3 bottomRight = camera.ViewportToWorldPoint(new Vector3(1, 0, camera.nearClipPlane));

          switch (anchorPosition)
        {
            case GridAnchorPosition.UpperLeft:
                break;
            case GridAnchorPosition.UpperMiddle:   
                break;
            case GridAnchorPosition.UpperRight: 
                break;
            case GridAnchorPosition.MiddleLeft:
                break;
            case GridAnchorPosition.Middle:
                break;
            case GridAnchorPosition.MiddleRight:
                break;
            case GridAnchorPosition.BottomLeft:
                break;
            case GridAnchorPosition.BottomMiddle:
                break;
            case GridAnchorPosition.BottomRight:
                break;

            default: break;
        }

        return gridPosition;
    }



    public Vector2 GetBounds()
    {
        return GetGridBounds(RowLenght,ColumnLenght, new Vector2(CellWidth, CellHeight));
    }

    private Vector2 GetGridBounds(float rowSize, float columnSize, Vector2 CellSize)
    {
        return new Vector2(columnSize * CellSize.x, rowSize * CellSize.y);
    }
    
}
