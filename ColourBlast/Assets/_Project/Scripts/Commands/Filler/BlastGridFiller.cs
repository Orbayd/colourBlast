
using System.Collections.Generic;
using System.Linq;
using ColourBlast.Commands.Fill;
using ColourBlast.Grid2D;
using ColourBlast.Helpers;

namespace ColourBlast
{
    public class BlastGridFiller : IFillCommand
    {
        public List<CellPosition> _emptyPositions;

        private IFillStrategy _strategy;

        public BlastGridFiller(IFillStrategy strategy)
        {
            _strategy = strategy;
        }

        public void Fill(AnimatedBlastGrid2D<BlastItem> grid)
        {
            _emptyPositions = grid.GetEmptyCells().ToList();
            var columns = grid.GroupByColumns(_emptyPositions);
            foreach (var column in columns)
            {
                var rowIds = column.Value.OrderByDescending(x => x);
                foreach (var row in rowIds)
                {
                    var blastItem = _strategy.Execute(grid, new CellPosition(row, column.Key));
                    grid.SetCell(row, column.Key, blastItem);
                }
            }
        }

    }
}