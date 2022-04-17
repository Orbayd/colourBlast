using ColourBlast.Commands.Fill;
using ColourBlast.Grid2D;

namespace ColourBlast
{
    public class SourceFiller : IFillStrategy
    {
        private AnimatedBlastGrid2D<BlastItem> _source;
        public SourceFiller(AnimatedBlastGrid2D<BlastItem> reserveGrid)
        {
            _source = reserveGrid;
        }
        public BlastItem Execute(AnimatedBlastGrid2D<BlastItem> grid, CellPosition position)
        {
            return _source.RemoveLastItemInColumn(position.Column);
        }
    }
}
