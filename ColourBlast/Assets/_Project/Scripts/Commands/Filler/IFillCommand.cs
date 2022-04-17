using ColourBlast.Grid2D;

namespace ColourBlast.Commands.Fill
{
    public interface IFillCommand
    {
        void Fill(AnimatedBlastGrid2D<BlastItem> grid);
    }
}