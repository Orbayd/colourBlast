using System;
using UnityEngine;

public class BlastItemFactory
{
    private GameObject _template;
    private Array _blastColours;
    private int _colourlimit;

    public BlastItemFactory(GameObject template, int colourlimit)
    {
        _blastColours = Enum.GetValues(typeof(BlastColour)); 
        _template = template;
        _colourlimit = colourlimit;
    }
    public BlastItem CreateRandom()
    {
        var blastItem = GameObject.Instantiate(_template).GetComponent<BlastItem>();
        blastItem.BlastColour = (BlastColour)_blastColours.GetValue(UnityEngine.Random.Range(0, Mathf.Clamp(_colourlimit,1,_blastColours.Length)));
        return blastItem;
    }
}