using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    [SerializeField]
    BlastGridConfig _config;

    [SerializeField]
    TMP_Text DebugTextTemplate;

    private BlastGrid2D<int> _grid;
    private BlastGrid2D<TMP_Text> _debugGrid;
    private BlastManager _blastManager;
    
    void Start()
    {
        _grid = new BlastGrid2D<int>(_config);
        _debugGrid = new BlastGrid2D<TMP_Text>(_config);
        _grid.SetPosition(this.transform.position);
        _grid.Build();
        // _grid.TraverseAll((x, y) =>
        // {
        //     var rand = UnityEngine.Random.Range(0, 6);
        //     _grid.SetCell(x, y, rand);
        //     CreateDebugObject(x, y, rand);

        // });
        // _grid.SetCell(0, 0, 2);
        // _grid.SetCell(0, 1, 2);
        // _grid.SetCell(0, 2, 0);
        // _grid.SetCell(1, 0, 1);
        // _grid.SetCell(1, 1, 2);
        // _grid.SetCell(1, 2, 5);
        // _grid.SetCell(2, 0, 2);
        // _grid.SetCell(2, 1, 5);
        // _grid.SetCell(2, 2, 5);

        _grid.SetCell(0, 0, 0);
        _grid.SetCell(0, 1, 1);
        _grid.SetCell(0, 2, 2);
        _grid.SetCell(1, 0, 1);
        _grid.SetCell(1, 1, 2);
        _grid.SetCell(1, 2, 0);
        _grid.SetCell(2, 0, 2);
        _grid.SetCell(2, 1, 0);
        _grid.SetCell(2, 2, 1);

         _grid.TraverseAll((x, y) =>
         {
            CreateDebugObject(x, y, _grid.GetCell(x,y));
         });

        _debugGrid.Build();
        _debugGrid.SetPosition(transform.position);
        _blastManager = new BlastManager();
        //FindGroups();
        UpdateDebugGrid();

        StartCoroutine(ShuffleRoutine());

    }

    private IEnumerator ShuffleRoutine()
    {
        yield return new WaitForSecondsRealtime(3);
        _blastManager.Shuffle(_grid);
        UpdateDebugGrid();
    }

    private void Update()
    {
        DrawGridBordersDebug();

        if (Input.GetMouseButtonDown(0))
        {
            var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var gridPosition = _grid.WorldToGridPosition(worldPosition);
            Debug.Log($"Grid Position [{gridPosition.x},{gridPosition.y}]");
            var group = _blastManager.Find(gridPosition.x, gridPosition.y);
            if (group != null && group.IsBlastable)
            {
                _blastManager.Collapse(_grid, _config.ColumnLenght, group);
                _blastManager.Fill(_grid);
                _blastManager.CreateBlastableGroups(_grid);
                //FindGroups();

                UpdateDebugGrid();
            }
        }
    }

    private void UpdateDebugGrid()
    {
        _grid.TraverseAll((x, y) =>
        {
            var value = _grid.GetCell(x, y);
            var text = _debugGrid.GetCell(x, y).text = value.ToString();
        });
        BlastableGroupHelper(_blastManager);
    }

    private void FindGroups()
    {
      
        BlastableGroupHelper(_blastManager);
    }

    private void BlastableGroupHelper(BlastManager blastManager)
    {
        foreach (var blastGroup in blastManager.BlastGroups)
        {
            if (blastGroup.IsBlastable)
            {
                var debugMsg = blastGroup.Value + string.Join(",", blastGroup.GetCellPositions().Select(x => $"[{x.Row},{x.Column}]"));
                Debug.Log(debugMsg);
                foreach (var cell in blastGroup.GetCellPositions())
                {
                    var text = _debugGrid.GetCell(cell.Row, cell.Column);
                    text.color = Color.green;
                }
            }
        }
    }

    private void CreateDebugObject(int x, int y, int value)
    {
        var DebugText = GameObject.Instantiate(DebugTextTemplate);
        DebugText.gameObject.SetActive(true);
        DebugText.text = value.ToString();
        DebugText.transform.position = _grid.GridToWorldPosition(x, y);
        DebugText.gameObject.name = $"[{x},{y}-{value}]";

        _debugGrid.SetCell(x, y, DebugText);
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