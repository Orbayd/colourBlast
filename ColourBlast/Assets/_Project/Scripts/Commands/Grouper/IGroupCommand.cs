using System.Collections.ObjectModel;
using ColourBlast.Grid2D;

public interface IGroupCommand
{
    void CreateGroups(AnimatedBlastGrid2D<BlastItem> _grid);
    ReadOnlyCollection<BlastGroup> BlastGroups {get;}
}
