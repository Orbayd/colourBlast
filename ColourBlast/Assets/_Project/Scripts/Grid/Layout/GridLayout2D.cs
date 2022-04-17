using UnityEngine;

namespace ColourBlast.Grid2D
{
    public class GridLayout2D
    {
        public GridAnchorPosition AnchorPosition;

        public bool IsFlexible { get; set; } = false;
        public bool IsFixedSize { get; set; } = false;
        public int RowLenght { get; }
        public int ColumnLenght { get; }
        private float _cellSize;
        public float CellHeight { get; private set; }
        public float CellWidth { get; private set; }

        public Vector2 Offset;

        private Vector3 _topLeft;
        private Vector3 _topRight;
        private Vector3 _bottomleft;
        private Vector3 _bottomRight;

        public GridLayout2D(int rowLenght, int columnLenght, float cellSize, bool IsFixedSize, bool IsFlexible)
        {
            RowLenght = rowLenght;
            ColumnLenght = columnLenght;
            _cellSize = cellSize;

            var camera = Camera.main;
            _topLeft = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane));
            _topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
            _bottomleft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
            _bottomRight = camera.ViewportToWorldPoint(new Vector3(1, 0, camera.nearClipPlane));

            CellWidth = Vector3.Distance(_topLeft, _topRight) / ColumnLenght;
            CellHeight = Vector3.Distance(_bottomRight, _topRight) / RowLenght;
            if (IsFixedSize)
            {
                CellHeight = Mathf.Min(CellHeight, _cellSize);
                CellWidth = Mathf.Min(CellWidth, _cellSize);
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
        // public Vector2Int WorldToGridPosition(Vector2 worldPosition)
        // {
        //     return WorldToGridPosition(AnchorPosition, worldPosition, new Vector2(CellWidth, CellHeight));
        // }

        Vector3 GetGridPosition(GridAnchorPosition anchorPosition, int row, int column, Vector2 cellSize)
        {
            Vector3 worldPositon = _topLeft;
            var bounds = GetGridBounds(RowLenght, ColumnLenght, cellSize);

            switch (anchorPosition)
            {
                case GridAnchorPosition.UpperLeft:
                    break;
                case GridAnchorPosition.UpperMiddle:
                    worldPositon = Vector3.Lerp(worldPositon, _topRight, 0.5f);
                    worldPositon -= new Vector3(bounds.x * 0.5f, 0, 0);
                    break;
                case GridAnchorPosition.UpperRight:
                    worldPositon = _topRight - new Vector3(bounds.x, 0, 0);
                    break;
                case GridAnchorPosition.MiddleLeft:
                    worldPositon = Vector3.Lerp(_topLeft, _bottomleft, 0.5f);
                    worldPositon += new Vector3(0, bounds.y * 0.5f, 0);
                    break;
                case GridAnchorPosition.Middle:
                    worldPositon = Vector3.Lerp(Vector3.Lerp(_topLeft, _bottomleft, 0.5f), Vector3.Lerp(_bottomRight, _topRight, 0.5f), 0.5f);
                    worldPositon += new Vector3(-bounds.x * 0.5f, bounds.y * 0.5f, 0);
                    break;
                case GridAnchorPosition.MiddleRight:
                    worldPositon = Vector3.Lerp(_topRight, _bottomRight, 0.5f);
                    worldPositon += new Vector3(-bounds.x, bounds.y * 0.5f, 0);
                    break;
                case GridAnchorPosition.BottomLeft:
                    worldPositon = _bottomleft;
                    worldPositon += new Vector3(0, bounds.y);
                    break;
                case GridAnchorPosition.BottomMiddle:
                    worldPositon = Vector3.Lerp(_bottomleft, _bottomRight, 0.5f);
                    worldPositon += new Vector3(-bounds.x * 0.5f, bounds.y);
                    break;
                case GridAnchorPosition.BottomRight:
                    worldPositon = _bottomRight - new Vector3(bounds.x, -bounds.y, 0);
                    break;

                default: break;
            }
            var position = new Vector3(worldPositon.x + column * cellSize.x, worldPositon.y - row * cellSize.y, 0) + new Vector3(Offset.x, Offset.y, 0);
            worldPositon = position + new Vector3(cellSize.x * 0.5f, -cellSize.y * 0.5f);

            return worldPositon;
        }

        //TODO
        Vector2Int WorldToGridPosition(GridAnchorPosition anchorPosition, Vector2 worldPosition, Vector2 cellSize)
        {
            Vector2Int gridPosition = new Vector2Int(-1, -1);
            Vector3 topleft = _topLeft;
            var bounds = GetGridBounds(RowLenght, ColumnLenght, cellSize);

            switch (anchorPosition)
            {
                case GridAnchorPosition.UpperLeft:
                    topleft = _topLeft + new Vector3(-cellSize.x * 0.5f, cellSize.y * 0.5f);
                    var rowId = Mathf.FloorToInt(bounds.x / (worldPosition.x - gridPosition.x));
                    var columnId = Mathf.FloorToInt(bounds.y / (worldPosition.y - gridPosition.x));
                    gridPosition = new Vector2Int(rowId, columnId);

                    //Debug.Log($"TopLeft{topleft},WorldPositon{worldPosition},GridPositon{gridPosition} CellSize {cellSize}");
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



        public Vector2 GetGridBounds()
        {
            return GetGridBounds(RowLenght, ColumnLenght, new Vector2(CellWidth, CellHeight));
        }

        private Vector2 GetGridBounds(float rowSize, float columnSize, Vector2 CellSize)
        {
            return new Vector2(columnSize * CellSize.x, rowSize * CellSize.y);
        }

    }
}