using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class Pixel : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;

    private Animator _animator;

    private Color _lastColor;

    private void Awake()
    {
        _lastColor = _sprite.color;
        _animator = GetComponent<Animator>();
    }

    internal void ChangeColor(Color newColor)
    {
        _lastColor = _sprite.color;
        _sprite.color = newColor;
        _animator.SetTrigger("Color");
    }

    internal void Undo()
    {
        _sprite.color = _lastColor;
        _animator.SetTrigger("Undo");
    }
}
