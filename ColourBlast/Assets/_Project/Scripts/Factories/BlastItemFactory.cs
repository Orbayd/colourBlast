using System;
using ColourBlast.Enums;
using ColourBlast.Helpers;
using UnityEngine;
using UnityEngine.U2D;

namespace ColourBlast
{
    public class BlastItemFactory : IFactory<BlastItem>
    {
        private Array _blastColours;
        private SpriteAtlas _atlas;
        private PoolingService _poolingService;
        private int _colourlimit;

        public BlastItemFactory(PoolingService poolingService, SpriteAtlas atlas, int colourlimit)
        {
            _blastColours = Enum.GetValues(typeof(BlastColour));
            _poolingService = poolingService;
            _colourlimit = colourlimit;
            _atlas = atlas;
        }
        public BlastItem Create()
        {
            return Create(Vector2.zero);
        }

        public BlastItem Create(Vector2 position)
        {
            var blastItem = _poolingService.Spawn(position, Vector3.zero).GetComponent<BlastItem>();
            blastItem.BlastColour = (BlastColour)_blastColours.GetValue(UnityEngine.Random.Range(0, Mathf.Clamp(_colourlimit, 1, _blastColours.Length)));
            blastItem.SetImage(_atlas.GetSprite($"{blastItem.BlastColour}_Default"));
            blastItem.gameObject.AddComponent<BoxCollider2D>();
            blastItem.GetComponent<BoxCollider2D>().isTrigger = true;
            return blastItem;
        }
    }
}