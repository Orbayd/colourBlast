using UnityEngine;

public interface IFactory<T>
{
    T Create();
    T Create(Vector2 position);
}
