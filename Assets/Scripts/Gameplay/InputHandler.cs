using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Color _nextColor;

    public delegate void InputAction(bool flag);
    public static InputAction UndoAvailable;

    private bool _checkForPixelHit;
    private Pixel _pixelForUndo;
    private Pixel _ignorePixelHit;

    private void Awake()
    {
        TouchSimulation.Enable();
    }

    private void Start()
    {
        UndoAvailable?.Invoke(false);
    }

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
        _ignorePixelHit = null;
    }

    public void Undo()
    {
        if (_pixelForUndo == null)
            return;

        _pixelForUndo.Undo();
        UpdateUndoAvailable(null);
        _ignorePixelHit = null;
    }

    private void UpdateUndoAvailable(Pixel pixel)
    {
        _pixelForUndo = pixel;
        UndoAvailable?.Invoke(pixel != null);
    }

    private void CheckForPixelHit()
    {
        var point = Camera.main.ScreenToWorldPoint(Touchscreen.current.position.ReadValue());
        var hit = Physics2D.Raycast(point, Vector2.zero, Mathf.Infinity, layerMask: LayerMask.GetMask("Pixel"));

        if (!hit)
            return;

        var pixel = hit.collider.GetComponent<Pixel>();

        if (pixel == _ignorePixelHit)
            return;

        pixel.ChangeColor(_nextColor);

        _ignorePixelHit = pixel;
        UpdateUndoAvailable(pixel);
    }

}
