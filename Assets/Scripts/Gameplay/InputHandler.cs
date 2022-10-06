using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Color _nextColor;

    private bool _checkForPixelHit;

    private void Update()
    {
        if (Touchscreen.current.primaryTouch.isInProgress)
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
        var point = Camera.main.ScreenToWorldPoint(Touchscreen.current.position.ReadValue());
        var hit = Physics2D.Raycast(point, Vector2.zero, Mathf.Infinity, layerMask: LayerMask.GetMask("Pixel"));

        if (hit)
        {
            var changeColor = hit.collider.GetComponent<Pixel>();
            changeColor.ChangeColor(_nextColor);
        }
    }
}
