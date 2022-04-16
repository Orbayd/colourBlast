using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridAnchorPosition
{
    UpperLeft,UpperMiddle,UpperRight,
    MiddleLeft,Middle,MiddleRight,
    BottomLeft,BottomMiddle,BottomRight
}


public class BlastGridManager : MonoBehaviour
{
    [SerializeField]
    private BlastGridConfig _config;

    [SerializeField]
    private Vector2 Space;

    [Header("Margins")]
    [SerializeField]
    private float Top;

    [SerializeField]
    private float Bottom;

    [SerializeField]
    private float Left;

    [SerializeField]
    private float Right;

    [SerializeField]
    private GridAnchorPosition AnchorPosition;

    [SerializeField]
    private bool IsFlexible;
    
    [SerializeField]
    private bool IsFixedSize;

    void OnDrawGizmosSelected()
    {
        var camera = Camera.main;

        Vector3 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(topRight, 0.1f);
        Gizmos.DrawRay(camera.transform.position, topRight - camera.transform.position);

        Vector3 bottomRight = camera.ViewportToWorldPoint(new Vector3(1, 0, camera.nearClipPlane));
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(bottomRight, 0.1f);
        Gizmos.DrawRay(camera.transform.position, bottomRight - camera.transform.position);

        Vector3 topleft = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane));
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(topleft, 0.1f);
        Gizmos.DrawRay(camera.transform.position, topleft - camera.transform.position);

        Vector3 bottomleft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(bottomleft, 0.1f);
        Gizmos.DrawRay(camera.transform.position, bottomleft - camera.transform.position);

        DrawGizmosGrid(Vector3.Distance(topleft, topRight) / _config.ColumnLenght, Vector3.Distance(bottomRight, topRight) / _config.RowLenght);
    }

    void DrawGizmosGrid(float cellWidth, float cellHeight)
    {    
        if(IsFixedSize)
        {
            cellHeight = Mathf.Min(cellHeight,_config.CellSize);
            cellWidth = Mathf.Min(cellWidth,_config.CellSize);
        }

        if(!IsFlexible)
        {
            cellHeight = Mathf.Min(cellHeight,cellWidth);
            cellWidth = Mathf.Min(cellHeight,cellWidth);
        }

        var camera = Camera.main;
        for (int i = 0; i < _config.ColumnLenght; i++)
        {
            for (int k = 0; k < _config.RowLenght; k++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(GetGridPosition(AnchorPosition,i,k,new Vector2(cellWidth,cellHeight)), new Vector3(cellWidth, cellHeight, 0) - new Vector3(Space.x,Space.y,0));
                if(i== 0 && k == 0)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(GetGridPosition(AnchorPosition,k,i,new Vector2(cellWidth,cellHeight)),0.1f);
                }
            }
        }
    }

    Vector3 GetGridPosition(GridAnchorPosition anchorPosition,int row, int column, Vector2 cellSize)
    {
        var camera = Camera.main;
        Vector3 gridposition = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane));
        var bounds = GetGridBounds(_config.RowLenght,_config.ColumnLenght, cellSize);
        
        Vector3 topLeft = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane));
        Vector3 topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
        Vector3 bottomleft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        Vector3 bottomRight = camera.ViewportToWorldPoint(new Vector3(1, 0, camera.nearClipPlane));
      
        switch (anchorPosition)
        {
            case GridAnchorPosition.UpperLeft : 
              
            break;
            case GridAnchorPosition.UpperMiddle : 
                gridposition = Vector3.Lerp(gridposition,topRight,0.5f);
                gridposition -= new Vector3(bounds.x * 0.5f,0,0);
            break;
            case GridAnchorPosition.UpperRight : 
                gridposition = topRight - new Vector3(bounds.x,0,0);     
            break;
            case GridAnchorPosition.MiddleLeft :
              
                gridposition = Vector3.Lerp(topLeft,bottomleft,0.5f);
                gridposition += new Vector3(0,bounds.y * 0.5f,0);
            break;
            case GridAnchorPosition.Middle :
               
                gridposition = Vector3.Lerp(Vector3.Lerp(topLeft,bottomleft,0.5f),Vector3.Lerp(bottomRight,topRight,0.5f),0.5f);
                gridposition += new Vector3(-bounds.x * 0.5f,bounds.y * 0.5f,0);
             
            break;
            case GridAnchorPosition.MiddleRight :              
                gridposition = Vector3.Lerp(topRight,bottomRight,0.5f);             
                gridposition += new Vector3(-bounds.x ,bounds.y * 0.5f,0);
            break;
            case  GridAnchorPosition.BottomLeft:
                gridposition = bottomleft;
                gridposition += new Vector3(0,bounds.y);
            break;
            case  GridAnchorPosition.BottomMiddle:
                gridposition = Vector3.Lerp(bottomleft,bottomRight,0.5f);   
                gridposition += new Vector3(-bounds.x * 0.5f,bounds.y);        
            break;
            case  GridAnchorPosition.BottomRight:
                gridposition = bottomRight - new Vector3(bounds.x,-bounds.y,0);
            break;
            
            default:break;
        }
        var position = new Vector3(gridposition.x + row * cellSize.x , gridposition.y - column * cellSize.y, 0);
        gridposition = position + new Vector3(cellSize.x * 0.5f, -cellSize.y * 0.5f);     
        
        return gridposition;
    }

    Vector2 GetGridBounds(float rowSize, float columnSize, Vector2 CellSize)
    {
        return new Vector2(columnSize * CellSize.x , rowSize * CellSize.y);
    }
}
