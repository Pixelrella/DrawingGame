using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private PixelGenerator _pixelGenerator;
    [SerializeField] private Round _round;
    [SerializeField] private ColorPicker _colorPicker;

    private DifficultyProgression _progression;

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        _progression = new DifficultyProgression(_pixelGenerator.GetNumberOfPixels);
        StartLevel();
    }

    public void StartLevel()
    {
        _pixelGenerator.Generate(_progression.GetGeneratorParameters());
        _colorPicker.SpawnColors();
        _round.RoundStart();
    }

    public void StartNextLevel()
    {
        _progression.Advance();
        StartLevel();
    }
}