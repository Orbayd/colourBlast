
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class BlastGroup
{
    public BlastColour Value;
    private List<CellPosition> _items = new List<CellPosition>();

    public bool IsBlastable => _items.Count > 1;

    public void Add(int row, int column)
    {
        _items.Add(new CellPosition() { Row = row, Column = column });
    }

    public void Add(CellPosition position)
    {
        _items.Add(position);
    }

    public void Remove(int row, int column)
    {
        _items.RemoveAll(x => x.Row == row && x.Column == column);
    }

    public void Remove(CellPosition position)
    {
        _items.RemoveAll(x => x.Row == position.Row && x.Column == position.Column);
    }

    public bool Contains(CellPosition position)
    {
        return _items.Any(x => x.Column == position.Column && x.Row == position.Row);
    }

    public bool Contains(int row, int column)
    {
        return _items.Any(x => x.Column == column && x.Row == row);
    }

    public void Clear()
    {
        _items.Clear();
    }

    // public Dictionary<int, List<int>> GroupByColumns()
    // {
    //     Dictionary<int, List<int>> colums = new Dictionary<int, List<int>>();
    //     foreach (var cellPosition in _items)
    //     {
    //         if (!colums.ContainsKey(cellPosition.Column))
    //         {
    //             colums.Add(cellPosition.Column, new List<int>());
    //         }

    //         colums[cellPosition.Column].Add(cellPosition.Row);
    //     }

    //     return colums;
    // }

    // public CellPosition RandomCell()
    // {
    //     var rand = UnityEngine.Random.Range(0, _items.Count);
    //     var randomCell = _items.ElementAt(rand);
    //     _items.Remove(randomCell);
    //     return randomCell;
    // }

    public ReadOnlyCollection<CellPosition> GetCellPositions()
    {
        return _items.AsReadOnly();
    }
}
