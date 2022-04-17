using UnityEngine;

namespace ColourBlast
{
    public interface IFactory<T>
    {
        T Create();
        T Create(Vector2 position);
    }
}