using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "BlastGroup", menuName = "ScriptableObjects/BlastGroup", order = 1)]
public class BlastGroupConfig : ScriptableObject
{
    [Min(1)]
    public int A;
    [Min(1)]
    public int B;
    [Min(1)]
    public int C;

    [Range(1,6)]
    public int ColourCount = 3; 
    
    public SpriteAtlas Atlast;
}
