using System;
using UnityEngine;

public class DifficultyProgression
{
    private int _cellSizeIndex;
    private int _currentLevel;
    private int _numberOfPixels;

    private float[] _cellSizeProgression = new float[] { 3f, 2f, 1f, 0.5f };
    private int[] _numLevelsPerCellSize = new int[] { 3, 3, 3, 50 };
    private int[] _maxNumberOfPixelsPerCellSize = new int[4];

    public DifficultyProgression(Func<float, int> getNumberOfPixels)
    {
        for (int i = 0; i < _cellSizeProgression.Length; i++)
        {
            _maxNumberOfPixelsPerCellSize[i] = getNumberOfPixels(_cellSizeProgression[i]);
            Debug.Log($"DifficultyProgression _maxNumberOfPixelsPerCellSize[{i}] = {_maxNumberOfPixelsPerCellSize[i]}");
        }

        Reset();
    }

    public void Reset()
    {
        _cellSizeIndex = 0;
        _currentLevel = 1; // level is 1 based

        InitNumberOfPixelsForCurrentLevel();
    }

    public void Advance()
    {
        if (IsLastLevelReached())
        {
            return;
        }

        _currentLevel++;
        if (_currentLevel > _numLevelsPerCellSize[_cellSizeIndex])
        {
            _currentLevel = 1; // level is 1 based
            _cellSizeIndex++;
        }

        InitNumberOfPixelsForCurrentLevel();
    }

    private bool IsLastLevelReached()
    {
        return _cellSizeIndex == _cellSizeProgression.Length - 1
                && _currentLevel == _numLevelsPerCellSize[_cellSizeIndex];
    }

    private void InitNumberOfPixelsForCurrentLevel()
    {
        const float maxFillPercentage = .7f;
        var maxPixelsOnCurrentCellSize = Mathf.FloorToInt(_maxNumberOfPixelsPerCellSize[_cellSizeIndex] * maxFillPercentage);
        Debug.Log($"InitNumberOfPixelsForCurrentLevel _cellSizeIndex = {_cellSizeIndex}");
        Debug.Log($"InitNumberOfPixelsForCurrentLevel maxNum = {_maxNumberOfPixelsPerCellSize[_cellSizeIndex]}");
        Debug.Log($"InitNumberOfPixelsForCurrentLevel maxPixelsOnCurrentCellSize = {maxPixelsOnCurrentCellSize}");

        _numberOfPixels = Mathf.FloorToInt(maxPixelsOnCurrentCellSize / _numLevelsPerCellSize[_cellSizeIndex] * _currentLevel);

        Debug.Log($"InitNumberOfPixelsForCurrentLevel _numberOfPixels = {_numberOfPixels}");
    }

    public (float, int) GetGeneratorParameters()
    {
        return (_cellSizeProgression[_cellSizeIndex], _numberOfPixels);
    }
}