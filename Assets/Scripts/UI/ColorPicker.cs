using System.Collections.Generic;
using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private List<ColorPaletteScriptableObject> _colorPalette;
    [SerializeField] private ColorButton _colorButtonPrefab;

    private List<ColorButton> selectableButtons = new List<ColorButton>();

    private void Awake()
    {
        var palette = _colorPalette[Random.Range(0, _colorPalette.Count)];

        foreach (var color in palette.Colors)
        {
            var button = Instantiate<ColorButton>(_colorButtonPrefab, transform);
            button.Init(PickColor, color);
            selectableButtons.Add(button);
        }

        PickColor(selectableButtons[0]);
    }

    public void PickColor(ColorButton button)
    {
        _inputHandler.SetNextColor(button.Color);
        SelectButton(button);
    }

    private void SelectButton(ColorButton button)
    {
        foreach (var selectableButton in selectableButtons)
        {
            selectableButton.SetSelected(selectableButton == button);
        }
    }
}
