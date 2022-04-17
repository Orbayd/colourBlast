using ColourBlast.Grid2D;

namespace ColourBlast.Commands.Shuffle
{
    public interface IShuffleCommand
    {
        void Shuffle(AnimatedBlastGrid2D<BlastItem> grid);
    }
}
