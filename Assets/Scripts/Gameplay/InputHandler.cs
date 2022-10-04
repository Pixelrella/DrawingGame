using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Color _nextColor;

    private void Update()
    {
        if (Mouse.current.leftButton.wasReleasedThisFrame)
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
            Debug.Log($"Hit {hit.collider.name}");
            var changeColor = hit.collider.GetComponent<Pixel>();
            changeColor.ChangeColor(_nextColor);
        }
    }
}
