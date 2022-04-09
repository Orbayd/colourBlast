
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class BlastGroup
{
    public class CellPosition
    {
        public int Row;
        public int Column;
    }
    public int Value;
    private List<CellPosition> _items = new List<CellPosition>();

    public bool IsBlastable => _items.Count > 1;

    public void Add(int row, int column)
    {    
        _items.Add(new CellPosition() { Row = row, Column = column });      
    }

    public void Remove(int row, int column)
    {
        _items.RemoveAll(x=> x.Row == row && x.Column == column);
    }

    public bool Contains(int row, int column)
    {
        return _items.Any(x => x.Column == column && x.Row == row);
    }

    public void Clear()
    {
        _items.Clear();
        Value = -1;
    }

    public Dictionary<int,List<int>> GetColumns()
    {
        Dictionary<int,List<int>> colums = new Dictionary<int, List<int>>();
        foreach (var cellPosition in _items)
        {
            if(!colums.ContainsKey(cellPosition.Column))
            {
                colums.Add(cellPosition.Column,new List<int>());
            }

            colums[cellPosition.Column].Add(cellPosition.Row);
        }

        return colums;
    }

    public CellPosition RandomCell()
    {
        var rand = UnityEngine.Random.Range(0,_items.Count); 
        var randomCell = _items.ElementAt(rand);
        _items.Remove(randomCell);
        return randomCell;
    }

    public ReadOnlyCollection<CellPosition> GetCellPositions()
    {
        return _items.AsReadOnly();
    }
}
