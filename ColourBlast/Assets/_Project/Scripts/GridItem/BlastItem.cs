using UnityEngine;

public class BlastItem : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _renderer;

    public BlastColour BlastColour;
    
    public void SetImage(Sprite sprite)
    {
        _renderer.sprite = sprite;
    }

    private void ReSize(Sprite sprite,float cellSize)
    {
       
    }
}

