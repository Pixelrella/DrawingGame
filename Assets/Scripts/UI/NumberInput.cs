using TMPro;
using UnityEngine;

public class NumberInput : MonoBehaviour
{
    [SerializeField] private DebugLevel _debugLevel;
    [SerializeField] private TMP_InputField _text;

    void Awake()
    {
        _text.text = _debugLevel.NumPixels.ToString();
    }
}
