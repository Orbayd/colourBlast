
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ColourBlast.Commands.Group;
using ColourBlast.Grid2D;

namespace ColourBlast
{
    public class BlastGridGrouper : IGroupCommand
    {
        readonly private List<BlastGroup> _blastGroups;
        public ReadOnlyCollection<BlastGroup> BlastGroups { get { return _blastGroups.AsReadOnly(); } }
        public BlastGridGrouper()
        {
            _blastGroups = new List<BlastGroup>();
        }

        public void CreateGroups(AnimatedBlastGrid2D<BlastItem> _grid)
        {
            _blastGroups.Clear();
            _grid.TraverseAll((positon) =>
           {
               var blastgroup = Find(positon);
               if (blastgroup is null)
               {
                   var value = _grid.GetCell(positon);
                   if (value != null)
                   {
                   //If not create new Group
                       blastgroup = new BlastGroup() { Value = value.BlastColour };
                       blastgroup.Add(positon);
                       _blastGroups.Add(blastgroup);
                       TraverseAndCollect(positon, _grid, blastgroup);
                   }
               }
           });

        }
        private bool IsCellSameColour(AnimatedBlastGrid2D<BlastItem> _grid, CellPosition position, BlastGroup group)
        {
            var rowNeigbor = _grid.GetCell(position);
            if (rowNeigbor is null)
            {
                return false;
            }
            if (group.Value == rowNeigbor.BlastColour)
            {
                if (!group.Contains(position))
                {
                    group.Add(position);
                    return true;
                }
            }
            return false;
        }

        private void TraverseAndCollect(CellPosition position, AnimatedBlastGrid2D<BlastItem> _grid, BlastGroup group)
        {
            if (position.Row < _grid.RowLenght - 1)
            {
                var right = position.Right;
                if (IsCellSameColour(_grid, position.Right, group))
                {
                    TraverseAndCollect(right, _grid, group);
                }
            }
            if (position.Column < _grid.ColumnLenght - 1)
            {
                var down = position.Down;
                if (IsCellSameColour(_grid, down, group))
                {
                    TraverseAndCollect(down, _grid, group);
                }
            }
            if (position.Row > 0)
            {
                var left = position.Left;
                if (IsCellSameColour(_grid, left, group))
                {
                    TraverseAndCollect(left, _grid, group);
                }
            }

            if (position.Column > 0)
            {
                var up = position.Up;
                if (IsCellSameColour(_grid, up, group))
                {
                    TraverseAndCollect(up, _grid, group);
                }
            }
        }



        public BlastGroup Find(int row, int column)
        {
            return _blastGroups.FirstOrDefault(x => x.Contains(row, column));
        }

        private BlastGroup Find(CellPosition pos)
        {
            return _blastGroups.FirstOrDefault(x => x.Contains(pos));
        }
    }
}