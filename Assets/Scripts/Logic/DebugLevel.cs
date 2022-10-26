using System;
using UnityEngine;

public class DebugLevel : MonoBehaviour
{
    [SerializeField] private PixelGenerator _pixelGenerator;
    [SerializeField] private Round _round;
    [SerializeField] private ColorPicker _colorPicker;

    private int _numPixels;
    private float _cellSize;

    public void ChangeNumber(string newNumberText)
    {
        if (Int32.TryParse(newNumberText, out int newNumber))
        {
            _numPixels = newNumber;
        }
    }

    public void ChangeCellSize(string newCellSizeText)
    {
        if (float.TryParse(newCellSizeText, out float newCellSize))
        {
            newCellSize = Mathf.Max(newCellSize, .1f);
            newCellSize = Mathf.Min(newCellSize, 3f);

            _cellSize = newCellSize;
        }
    }

    public void StartLevel()
    {
        _pixelGenerator.Generate((_cellSize, _numPixels));
        _colorPicker.SpawnColors();
        _round.RoundStart();
    }
}