using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Color _nextColor;

    private bool _checkForPixelHit;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _checkForPixelHit = true;
            return;
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _checkForPixelHit = false;
            return;
        }

        if (_checkForPixelHit)
        {
            CheckForPixelHit();
        }
    }

    public void SetNextColor(Color color)
    {
        _nextColor = color;
    }

    private void CheckForPixelHit()
    {
        var point = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var hit = Physics2D.Raycast(point, Vector2.zero, Mathf.Infinity, layerMask: LayerMask.GetMask("Pixel"));

        if (hit)
        {
            var changeColor = hit.collider.GetComponent<Pixel>();
            changeColor.ChangeColor(_nextColor);
        }
    }
}
