using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Pixel : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;

    internal void ChangeColor(Color newColor)
    {
        _sprite.color = newColor;
    }
}
