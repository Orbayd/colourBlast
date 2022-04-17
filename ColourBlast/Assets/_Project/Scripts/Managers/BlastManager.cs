
using System.Collections.Generic;
using System.Linq;
using ColourBlast.Grid2D;
using ColourBlast.Helpers;
using UnityEngine;

public class BlastManager
{
    private BlastGroupConfig _blastConfig;
    private IFactory<BlastItem> _factory;
    private IShuffleCommand _shuffler;
    private ICollapseCommand _collapser;
    private IFillCommand _filler;
    private IGroupCommand _grouper;

    public BlastManager(BlastGroupConfig blastConfig, IFactory<BlastItem> factory,IGroupCommand grouper, ICollapseCommand collpaser, IFillCommand filler,IShuffleCommand shuffler)
    {
        _blastConfig = blastConfig;
        _factory = factory;

        _shuffler = shuffler;
        _filler = filler;
        _collapser = collpaser;
        _grouper = grouper;
    }

    public BlastGroup Find(int row, int column)
    {
       return _grouper.BlastGroups.FirstOrDefault(x=> x.Contains(row,column));
    }
    public void CreateBlastItems(AnimatedBlastGrid2D<BlastItem> _grid)
    {
        _grid.TraverseAll((postion) =>
        {
            var item = _factory.Create();
            _grid.SetCell(postion.Row, postion.Column, item);
            item.transform.position = _grid.GridToWorldPosition(postion.Row,postion.Column);
            item.name = $"GridItem_[{postion.Row},{postion.Column}]";
            var sprite = item.GetComponent<SpriteRenderer>().sprite;
            var layout = _grid.GridLayout;
            item.transform.localScale = new Vector3(layout.CellWidth / sprite.bounds.size.x , layout.CellHeight / sprite.bounds.size.x, 1); 
        });

    }
    public void CreateBlastGroups(AnimatedBlastGrid2D<BlastItem> grid)
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

    public void Shuffle(AnimatedBlastGrid2D<BlastItem> grid)
    {
        _shuffler.Shuffle(grid);
    }
    public bool HasBlastable()
    {
        return _grouper.BlastGroups.Any(x => x.IsBlastable);
    }

    private void SetBlastGroupSprites(AnimatedBlastGrid2D<BlastItem> grid)
    {
     
        foreach (var blastGroup in _grouper.BlastGroups)
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
