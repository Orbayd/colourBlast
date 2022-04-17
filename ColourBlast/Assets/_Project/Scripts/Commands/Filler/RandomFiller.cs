using ColourBlast.Grid2D;

public class RandomFiller : IFillStrategy
{
    private IFactory<BlastItem> _factory;
    public RandomFiller(IFactory<BlastItem> factory)
    {
        _factory = factory;
    }
    public BlastItem Execute(AnimatedBlastGrid2D<BlastItem> grid,CellPosition position)
    {
        var blastItem = _factory.Create(grid.GridToWorldPosition(position));
        // blastItem.transform.position = grid.GridToWorldPosition(position.Row,position.Column);
        return blastItem;
    }
}
