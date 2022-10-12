using TMPro;
using UnityEngine;

public class SizeInput : MonoBehaviour
{
    [SerializeField] private PixelGenerator _pixelGenerator;
    [SerializeField] private TMP_InputField _text;

    void Awake()
    {
        _text.text = _pixelGenerator.CellSize.ToString();
    }
}
