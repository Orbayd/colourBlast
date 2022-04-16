
using System.Collections.Generic;
using ColourBlast.Grid2D;

public interface ICollapseCommand
{
    void Collapse(AnimatedBlastGrid2D<BlastItem> grid, IEnumerable<CellPosition> source);
}
