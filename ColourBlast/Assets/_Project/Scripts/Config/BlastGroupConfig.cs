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
    public SpriteAtlas Atlast;
}
