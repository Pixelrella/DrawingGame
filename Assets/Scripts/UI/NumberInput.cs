using TMPro;
using UnityEngine;

public class NumberInput : MonoBehaviour
{
    [SerializeField] private PixelGenerator _pixelGenerator;
    [SerializeField] private TMP_InputField _text;

    void Awake()
    {
        _text.text = _pixelGenerator.Number.ToString();
    }
}
