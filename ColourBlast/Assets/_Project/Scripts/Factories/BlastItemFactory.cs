using System;
using ColourBlast.Helpers;
using UnityEngine;
using UnityEngine.U2D;

public interface IBlastItemFactory
{
    BlastItem CreateRandom();
    BlastItem CreateRandom(Vector2 position);
}

public class BlastItemFactory : IBlastItemFactory
{
    private GameObject _template;
    private Array _blastColours;
    private SpriteAtlas _atlas;

    private PoolingService _poolingService;
    private int _colourlimit;

    public BlastItemFactory(PoolingService poolingService ,SpriteAtlas atlas, int colourlimit)
    {
        _blastColours = Enum.GetValues(typeof(BlastColour)); 
        _poolingService = poolingService;
        //_template = template;
        _colourlimit = colourlimit;
        _atlas = atlas;
    }
    public BlastItem CreateRandom()
    {
        return CreateRandom(Vector2.zero);
    }

    public BlastItem CreateRandom(Vector2 position)
    {
        var blastItem = _poolingService.Spawn(position,Vector3.zero).GetComponent<BlastItem>();
        blastItem.BlastColour = (BlastColour)_blastColours.GetValue(UnityEngine.Random.Range(0, Mathf.Clamp(_colourlimit,1,_blastColours.Length)));
        blastItem.SetImage(_atlas.GetSprite($"{blastItem.BlastColour}_Default"));
        blastItem.gameObject.AddComponent<BoxCollider2D>();
        blastItem.GetComponent<BoxCollider2D>().isTrigger = true;
        return blastItem;
    }
}