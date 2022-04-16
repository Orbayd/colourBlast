using ColourBlast.Grid2D;

public interface IFillStrategy
{
    BlastItem Execute(AnimatedBlastGrid2D<BlastItem> grid, CellPosition position);
}
