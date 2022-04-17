
using ColourBlast.Grid2D;
using UnityEngine;

namespace ColourBlast
{
    public class Grid2DFactory : IFactory<AnimatedBlastGrid2D<BlastItem>>
    {
        private BlastGridConfig _config;

        public Grid2DFactory(BlastGridConfig config)
        {
            _config = config;
        }

        public AnimatedBlastGrid2D<BlastItem> Create()
        {
           return Create(Vector2.zero);
        }

        public AnimatedBlastGrid2D<BlastItem> Create(Vector2 position)
        {
            var gridLayout = new GridLayout2D(_config.RowLenght, _config.ColumnLenght, _config.CellSize, _config.IsFixedSize, _config.IsFlexible);
            gridLayout.AnchorPosition = _config.AnchorPosition;
            gridLayout.Offset = position;
            return new AnimatedBlastGrid2D<BlastItem>(new BlastGrid2D<BlastItem>(_config), gridLayout);
        }
    }
}