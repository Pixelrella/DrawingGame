using System.Collections.Generic;
using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;

    private List<ColorButton> selectableButtons = new List<ColorButton>();

    private void Awake()
    {
        var buttons = GetComponentsInChildren<ColorButton>();
        foreach (var button in buttons)
        {
            button.Init(PickColor);
            selectableButtons.Add(button);
        }
    }

    public void PickColor(ColorButton button)
    {
        _inputHandler.SetNextColor(button.Color);

        foreach(var selectableButton in selectableButtons)
        {
            selectableButton.SetSelected(selectableButton == button);
        }
    }
}
