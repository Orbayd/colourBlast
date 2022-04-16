using ColourBlast.Grid2D;

public class RandomFiller : IFillStrategy
{
    private IBlastItemFactory _factory;
    public RandomFiller(IBlastItemFactory factory)
    {
        _factory = factory;
    }
    public BlastItem Execute(AnimatedBlastGrid2D<BlastItem> grid,CellPosition position)
    {
        var blastItem = _factory.CreateRandom();
        blastItem.transform.position = grid.GridToWorldPosition(position.Row,position.Column);
        return blastItem;
    }
}
