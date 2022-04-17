using ColourBlast.Grid2D;

namespace ColourBlast.Commands.Fill
{
    public interface IFillStrategy
    {
        BlastItem Execute(AnimatedBlastGrid2D<BlastItem> grid, CellPosition position);
    }
}