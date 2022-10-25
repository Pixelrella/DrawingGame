using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Color _nextColor;
    [SerializeField] private Round _round;

    public delegate void InputAction(bool flag);
    public static InputAction UndoAvailable;

    private bool _checkForPixelHit;
    private Pixel _pixelForUndo;
    private Pixel _ignorePixelHit;

    private bool _isInputEnabled;

    private void Awake()
    {
        TouchSimulation.Enable();
        _round.RoundEnd += DisableInput;
        _round.RoundStart += EnableInput;
    }

    private void Start()
    {
        UndoAvailable?.Invoke(false);
    }

    private void DisableInput()
    {
        _isInputEnabled = false;
    }

    private void EnableInput()
    {
        _isInputEnabled = true;
    }

    private void Update()
    {
        if (!_isInputEnabled)
        {
            return;
        }

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
        _round.TryAddColoredCell(pixel);

        _ignorePixelHit = pixel;
        UpdateUndoAvailable(pixel);
    }

    private void OnDestroy()
    {
        _round.RoundEnd -= DisableInput;
        _round.RoundStart -= EnableInput;
    }
}
