using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    [SerializeField]
    BlastGridConfig _config;

    [SerializeField]
    BlastGroupConfig _blastgroupConfig;

    [SerializeField]
    private GameObject _template;

    private AnimatedBlastGrid2D<BlastItem> _grid;
    private AnimatedBlastGrid2D<BlastItem> _reserveGrid;
    private BlastManager _blastManager;
    private BlastManager _reserveManager;
    
    void Start()
    {
        InitGrid();
        InitCamera();
    }

    private void InitGrid()
    {
        var blastColours = Enum.GetValues(typeof(BlastColour));
        _grid = new AnimatedBlastGrid2D<BlastItem>(new BlastGrid2D<BlastItem>(_config));
        _grid.SetPosition(new Vector2(0, 0));

        _reserveGrid = new AnimatedBlastGrid2D<BlastItem>(new BlastGrid2D<BlastItem>(_config));
        _reserveGrid.SetPosition(new Vector2(0, 0) - (new Vector2(0, (_grid.CellSize * _grid.ColumnLenght) + _grid.CellSize)));

        var factory = new BlastItemFactory(_template,_blastgroupConfig.Atlast,_blastgroupConfig.ColourCount);
        _blastManager = new BlastManager(_blastgroupConfig,factory);

        _blastManager.CreateBlastGrid(_grid);
        _blastManager.CreateGroups(_grid);

        _reserveManager = new BlastManager(_blastgroupConfig,factory);
        _reserveManager.CreateBlastGrid(_reserveGrid);

        if(!_blastManager.HasBlastable())
        {
            StartCoroutine(ShuffleRoutine());
        }

    }

    private void InitCamera()
    {
        var verticalExtend  = Camera.main.orthographicSize * 2;
        var horizontalExtend = verticalExtend * Screen.width / Screen.height;

        var gridVerticalExtend =  (float)_grid.ColumnLenght * _grid.CellSize ;
        var gridHorizontalExtend = (float)_grid.RowLenght * _grid.CellSize ;

        if(gridHorizontalExtend > horizontalExtend)
        {
            Camera.main.orthographicSize *= gridHorizontalExtend / horizontalExtend;
        }
        if(gridVerticalExtend > verticalExtend)
        {
            Camera.main.orthographicSize *= gridVerticalExtend / verticalExtend;
        }

        verticalExtend  = Camera.main.orthographicSize;
        horizontalExtend = Screen.width / Screen.height * verticalExtend;

        Camera.main.transform.position = new Vector3(gridHorizontalExtend / 2f - _grid.CellSize / 2f,
                                                      - verticalExtend +_grid.CellSize / 2f , -10);
    
    }

    private IEnumerator ShuffleRoutine()
    {
        yield return new WaitForSecondsRealtime(3);
        _blastManager.Shuffle(_grid);
        _blastManager.CreateGroups(_grid);
    }

    private IEnumerator CollapseRoutine(BlastGroup group)
    {
        _blastManager.Collapse(_grid, group);
        yield return new WaitForSecondsRealtime(3);
        _blastManager.FillFromSource(_grid,_reserveGrid);
        _blastManager.CreateGroups(_grid);
        
        _reserveManager.Collapse(_reserveGrid);
        //yield return new WaitForSecondsRealtime(1);
        _reserveManager.Fill(_reserveGrid);
        if (!_blastManager.HasBlastable())
        {
            StartCoroutine(ShuffleRoutine());
        }
    }

    private void Update()
    {
        DrawGridBordersDebug();

        if (Input.GetMouseButtonDown(0))
        {
            var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var gridPosition = _grid.WorldToGridPosition(worldPosition);
            Debug.Log($"WorldPosition {worldPosition} Grid Position [{gridPosition.x},{gridPosition.y}]");
            var group = _blastManager.Find(gridPosition.x, gridPosition.y);
            if (group != null && group.IsBlastable)
            {
                StartCoroutine(CollapseRoutine(group));
            }
        }
    }

    
    private void DrawGridBordersDebug()
    {
        var k = _grid.CellSize * 0.5f;
        _grid.TraverseAll((x, y) =>
        {
            UnityEngine.Debug.DrawLine(_grid.GridToWorldPosition(x, y) - new Vector2(k, k), _grid.GridToWorldPosition(x, y + 1) - new Vector2(k, k), Color.red, _grid.CellSize);
            UnityEngine.Debug.DrawLine(_grid.GridToWorldPosition(x, y) + new Vector2(k, k), _grid.GridToWorldPosition(x + 1, y) + new Vector2(k, k), Color.red, _grid.CellSize);
        });
        UnityEngine.Debug.DrawLine(_grid.GridToWorldPosition(0,0) + new Vector2(-k, k), _grid.GridToWorldPosition(_config.ColumnLenght, 0) + new Vector2(-k, k), Color.red, _grid.CellSize);
        UnityEngine.Debug.DrawLine(_grid.GridToWorldPosition(0, 0) + new Vector2(-k, k), _grid.GridToWorldPosition(0, _config.RowLenght) + new Vector2(-k, k), Color.red, _grid.CellSize);
    }
    
}
