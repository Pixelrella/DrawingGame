using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ColorButton : MonoBehaviour
{
    [SerializeField] private GameObject _selectionFrame;
    [SerializeField] private Image _imageWithColor;

    private Button _button;
    private Action<ColorButton> _colorChangeCallback;

    public Color Color => _imageWithColor.color;

    public void Init(Action<ColorButton> colorChangeCallback, Color color)
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Select);
        _colorChangeCallback = colorChangeCallback;

        _imageWithColor.color = color;

        SetSelected(false);
    }

    private void Select()
    {
        _colorChangeCallback.Invoke(this);
    }

    public void SetSelected(bool isSelected)
    {
        _selectionFrame.SetActive(isSelected);
    }
}
