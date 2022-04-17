using ColourBlast.Commands.Fill;
using ColourBlast.Grid2D;

namespace ColourBlast
{
    public class RandomFiller : IFillStrategy
    {
        private IFactory<BlastItem> _factory;
        public RandomFiller(IFactory<BlastItem> factory)
        {
            _factory = factory;
        }
        public BlastItem Execute(AnimatedBlastGrid2D<BlastItem> grid, CellPosition position)
        {
            var blastItem = _factory.Create(grid.GridToWorldPosition(position));
            return blastItem;
        }
    }
}
