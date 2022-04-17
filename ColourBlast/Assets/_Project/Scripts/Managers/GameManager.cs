using System.Collections;
using UnityEngine;
using ColourBlast.Grid2D;
using ColourBlast.Helpers;
using ColourBlast.Enums;
using ColourBlast.Commands.Group;

namespace ColourBlast
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        BlastGridConfig _config;
        [SerializeField]
        BlastGroupConfig _blastgroupConfig;
        private AnimatedBlastGrid2D<BlastItem> _grid;
        private AnimatedBlastGrid2D<BlastItem> _reserveGrid;
        private BlastManager _blastManager;
        private BlastManager _reserveManager;

        [SerializeField]
        private GridAnchorPosition AnchorPosition;

        [SerializeField]
        private bool IsFlexible;

        [SerializeField]
        private bool IsFixedSize;

        private bool IsProcessing = false;

        void Start()
        {
            InitGrid();
        }
        void OnEnable()
        {
            AddEvents();
        }

        void OnDisable()
        {
            RemoveEvents();
        }

        private void InitGrid()
        {
            var poolService = new PoolingService((_config.RowLenght * _config.ColumnLenght) * 2, _blastgroupConfig.Template);
            poolService.Init();


            var gridFactory = new Grid2DFactory(_config);
            _grid = gridFactory.Create();


            _reserveGrid = gridFactory.Create();
            _reserveGrid.GridLayout.Offset = new Vector2(0, _reserveGrid.GridLayout.GetGridBounds().y);

            var factory = new BlastItemFactory(poolService, _blastgroupConfig.Atlast, _blastgroupConfig.ColourCount);

            var grouper = new BlastGridGrouper();
            var shuffler = new BlastGridShuffler();
            var collpaser = new BlastGridCollapser(poolService);
            var sourceFiller = new BlastGridFiller(new SourceFiller(_reserveGrid));
            _blastManager = new BlastManager(_blastgroupConfig, factory, grouper, collpaser, sourceFiller, shuffler);

            _blastManager.CreateBlastItems(_grid);
            _blastManager.CreateBlastGroups(_grid);

            var randomFiller = new BlastGridFiller(new RandomFiller(factory));
            _reserveManager = new BlastManager(_blastgroupConfig, factory, grouper, collpaser, randomFiller, shuffler);

            _reserveManager.CreateBlastItems(_reserveGrid);

            _grid.AllAnimationsCompleted += () =>
            {
                IsProcessing = false;
            };

            if (!_blastManager.HasBlastable())
            {
                StartCoroutine(ShuffleRoutine());
            }
        }

        float interval = 1/*1/60 * 2 */;
        float colldown = 1 / 60 * 2;

        public bool IsDebugMode;
        private void Update()
        {
            if (!IsDebugMode)
            {
                return;
            }
            colldown -= Time.deltaTime;
            if (colldown <= 0.0f)
            {
                colldown = interval;
                var rowId = UnityEngine.Random.Range(0, _grid.RowLenght);
                var columnId = UnityEngine.Random.Range(0, _grid.ColumnLenght);


                OnCellClicked(new OnClickEventHandler(new CellPosition(rowId, columnId)));

            }
        }
        private IEnumerator ShuffleRoutine()
        {
            IsProcessing = true;
            yield return new WaitForSecondsRealtime(0.5f);
            _blastManager.Shuffle(_grid);
            _blastManager.CreateBlastGroups(_grid);
        }

        private IEnumerator CollapseRoutine(BlastGroup group)
        {
            IsProcessing = true;
            _blastManager.Collapse(_grid, group);
            _blastManager.Fill(_grid);
            _blastManager.CreateBlastGroups(_grid);

            _reserveManager.Collapse(_reserveGrid);
            _reserveManager.Fill(_reserveGrid);
            yield return null;
            if (!_blastManager.HasBlastable())
            {
                StartCoroutine(ShuffleRoutine());
            }
        }

        private void OnCellClicked(OnClickEventHandler e)
        {
            if (IsProcessing)
            {
                return;
            }
            var position = e.Positon;
            var group = _blastManager.Find(position.Row, position.Column);
            if (group != null && group.IsBlastable)
            {
                StartCoroutine(CollapseRoutine(group));
            }
        }

        private void AddEvents()
        {
            MessageBus.Subscribe<OnClickEventHandler>((e) => OnCellClicked(e));
        }

        private void RemoveEvents()
        {
            MessageBus.UnSubscribe<OnClickEventHandler>();
        }

        void OnDrawGizmosSelected()
        {
            DebugHelpers.GizmosCameraCorners();
            DebugHelpers.GizmosGrid(_grid);
        }
    }
}
