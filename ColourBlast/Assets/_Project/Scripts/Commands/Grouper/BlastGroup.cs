
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class BlastGroup
{
    public BlastColour Value;
    private List<CellPosition> _items = new List<CellPosition>();

    public bool IsBlastable => _items.Count > 1;

    public void Add(CellPosition position)
    {
        _items.Add(position);
    }
    public void Remove(CellPosition position)
    {
        _items.Remove(position);
    }

    public bool Contains(CellPosition position)
    {
        return _items.Contains(position);
    }

    public void Add(int row, int column)
    {
        _items.Add(new CellPosition(row,column));
    }

    public void Remove(int row, int column)
    {
        _items.RemoveAll(x => x.Row == row && x.Column == column);
    }

    public bool Contains(int row, int column)
    {
        return _items.Any(x => x.Column == column && x.Row == row);
    }

    public CellPosition Find(int row, int column)
    {
        return _items.Find(x=> x.Row == row && x.Column == column);
    }

    public ReadOnlyCollection<CellPosition> GetCellPositions()
    {
        return _items.AsReadOnly();
    }
}
