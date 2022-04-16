using UnityEngine;
using ColourBlast.Helpers;
public interface IGridItem
{
    CellPosition Position {get; set;}
}

public abstract class GridItem : MonoBehaviour, IGridItem
{
    public CellPosition Position { get ; set; }
}

public class BlastItem : GridItem
{
    [SerializeField]
    private SpriteRenderer _renderer;

    public BlastColour BlastColour;

    public void SetImage(Sprite sprite)
    {
        _renderer.sprite = sprite;
    }

    void OnMouseDown()
    {
        MessageBus.Publish(new OnClickEventHandler(Position));
    }
}

