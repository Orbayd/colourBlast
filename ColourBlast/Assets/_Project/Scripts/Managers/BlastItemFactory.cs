using System;
using UnityEngine;
using UnityEngine.U2D;

public class BlastItemFactory
{
    private GameObject _template;
    private Array _blastColours;
    private SpriteAtlas _atlas;

    private int _colourlimit;

    public BlastItemFactory(GameObject template,SpriteAtlas atlas, int colourlimit)
    {
        _blastColours = Enum.GetValues(typeof(BlastColour)); 
        _template = template;
        _colourlimit = colourlimit;
        _atlas = atlas;
    }
    public BlastItem CreateRandom()
    {
        var blastItem = GameObject.Instantiate(_template).GetComponent<BlastItem>();
        blastItem.BlastColour = (BlastColour)_blastColours.GetValue(UnityEngine.Random.Range(0, Mathf.Clamp(_colourlimit,1,_blastColours.Length)));
        blastItem.SetImage(_atlas.GetSprite($"{blastItem.BlastColour}_Default"));
        return blastItem;
    }
}