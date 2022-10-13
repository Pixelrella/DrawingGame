using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Pixel : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;

    private Color _lastColor;

    private void Awake()
    {
        _lastColor = _sprite.color;
    }

    internal void ChangeColor(Color newColor)
    {
        _lastColor = _sprite.color;
        _sprite.color = newColor;
    }

    internal void Undo()
    {
        _sprite.color = _lastColor;
    }
}
