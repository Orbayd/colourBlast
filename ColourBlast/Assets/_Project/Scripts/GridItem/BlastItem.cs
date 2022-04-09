using UnityEngine;

public class BlastItem : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _renderer;
    
    public void SetImage(Sprite sprite)
    {
        _renderer.sprite = sprite;
    }
}
