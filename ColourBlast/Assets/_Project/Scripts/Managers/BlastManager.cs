
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlastManager
{
    private BlastGroupConfig _blastConfig;
    private BlastItemFactory _factory;
    private List<BlastGroup> _blastGroups = new List<BlastGroup>();
    private BlastGridShuffler _shuffler;
    private BlastGridCollapser _collapser;
    private BlastGridFiller _filler;
    private BlastGridGrouper _grouper;

    public BlastManager(BlastGroupConfig blastConfig, BlastItemFactory factory)
    {
        _blastConfig = blastConfig;
        _factory = factory;

        _shuffler = new BlastGridShuffler();
        _collapser = new BlastGridCollapser();
        _filler = new BlastGridFiller(_factory);
        _grouper = new BlastGridGrouper(_blastGroups);
    }
    public BlastGroup Find(int row, int column)
    {
       return _grouper.Find(row,column);
    }
    public void CreateBlastGrid(AnimatedBlastGrid2D<BlastItem> _grid)
    {
        _grid.TraverseAll((row, column) =>
        {
            var item = _factory.CreateRandom();
            _grid.SetCell(row, column, item);
            item.transform.position = _grid.GridToWorldPosition(row,column);
            item.name = $"GridItem_[{row},{column}]";
            var sprite = item.GetComponent<SpriteRenderer>().sprite;
            var layout = _grid.GridLayout;
            item.transform.localScale = new Vector3(layout.CellWidth / sprite.bounds.size.x , layout.CellHeight / sprite.bounds.size.x, 1); 
        });

    }
    public void CreateGroups(AnimatedBlastGrid2D<BlastItem> grid)
    {
        _grouper.CreateGroups(grid);
        SetBlastGroupSprites(grid);

    }
    public void Collapse(AnimatedBlastGrid2D<BlastItem> grid,BlastGroup group)
    {
        _collapser.Collapse(grid, group.GetCellPositions());
    }

    public void Collapse(AnimatedBlastGrid2D<BlastItem> grid)
    {
        _collapser.Collapse(grid,grid.GetEmptyCells());
    }
    public void Fill(AnimatedBlastGrid2D<BlastItem> grid)
    {
        _filler.Fill(grid);
    }
    public void FillFromSource(AnimatedBlastGrid2D<BlastItem> grid , AnimatedBlastGrid2D<BlastItem> reservegrid)
    {
        _filler.Fill(grid,reservegrid);
    }

    public void Shuffle(AnimatedBlastGrid2D<BlastItem> grid)
    {
        _shuffler.Shuffle(grid);
    }
    public bool HasBlastable()
    {
        return _blastGroups.Any(x => x.IsBlastable);
    }

    private void SetBlastGroupSprites(AnimatedBlastGrid2D<BlastItem> grid)
    {
     
        foreach (var blastGroup in _blastGroups)
        {
            var positions = blastGroup.GetCellPositions();
            Sprite sprite;
          
            if (positions.Count <= _blastConfig.A)
            {
                sprite = _blastConfig.Atlast.GetSprite($"{blastGroup.Value}_Default");
            }
            else if (positions.Count <= _blastConfig.B)
            {
                sprite = _blastConfig.Atlast.GetSprite($"{blastGroup.Value}_A");
            }
            else if (positions.Count <= _blastConfig.C)
            {
                sprite = _blastConfig.Atlast.GetSprite($"{blastGroup.Value}_B");
            }
            else
            {
                sprite = _blastConfig.Atlast.GetSprite($"{blastGroup.Value}_C");
            }

            foreach (var position in positions)
            {
                var blastitem = grid.GetCell(position.Row, position.Column);
                blastitem.SetImage(sprite);
            }
        }

    }
}
