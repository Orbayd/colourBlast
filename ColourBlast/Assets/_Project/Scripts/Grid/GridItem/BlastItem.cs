using UnityEngine;
using ColourBlast.Helpers;

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

