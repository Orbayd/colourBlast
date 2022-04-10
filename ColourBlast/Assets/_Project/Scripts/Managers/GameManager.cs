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
    private BlastManager _blastManager;
    
    public int ColourCount = 3; // Game Config

    void Start()
    {
        InitGrid();

        _blastManager = new BlastManager(_blastgroupConfig,new BlastItemFactory(_template,ColourCount));
        _blastManager.CreateBlastGrid(_grid);
        _blastManager.CreateBlastableGroups(_grid);
        if(!_blastManager.HasBlastable())
        {
            StartCoroutine(ShuffleRoutine());
        }
    }

    private void InitGrid()
    {
        var blastColours = Enum.GetValues(typeof(BlastColour)); 
        _grid = new AnimatedBlastGrid2D<BlastItem>(new BlastGrid2D<BlastItem>(_config));
        _grid.SetPosition(this.transform.position);

        Camera.main.transform.position = new Vector3((float)_grid.RowLenght * _grid.CellSize / 2f , - (float)_grid.ColumnLenght/ _grid.CellSize * 2f,-10);
      
    }

    private BlastItem CreateBlastItem(int row, int column, Array blastColours)
    {
        var blastItem = GameObject.Instantiate(_template).GetComponent<BlastItem>();
        blastItem.transform.SetPositionAndRotation(_grid.GridToWorldPosition(row, column), Quaternion.identity);
        blastItem.BlastColour = (BlastColour)blastColours.GetValue(UnityEngine.Random.Range(0, Mathf.Clamp(ColourCount,1,blastColours.Length)));
        return blastItem;
    }

    private IEnumerator ShuffleRoutine()
    {
        yield return new WaitForSecondsRealtime(3);
        _blastManager.Shuffle(_grid);
        _blastManager.CreateBlastableGroups(_grid);
    }

    private IEnumerator CollapseRoutine(BlastGroup group)
    {
        _blastManager.Collapse(_grid, _config.ColumnLenght, group);
        yield return new WaitForSecondsRealtime(3);
        _blastManager.Fill(_grid);
        _blastManager.CreateBlastableGroups(_grid);
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
        UnityEngine.Debug.DrawLine(_grid.GridToWorldPosition(0,0) + new Vector2(-k, k), _grid.GridToWorldPosition(_config.RowLenght, 0) + new Vector2(-k, k), Color.red, _grid.CellSize);
        UnityEngine.Debug.DrawLine(_grid.GridToWorldPosition(0, 0) + new Vector2(-k, k), _grid.GridToWorldPosition(0, _config.ColumnLenght) + new Vector2(-k, k), Color.red, _grid.CellSize);
    }
    
}
