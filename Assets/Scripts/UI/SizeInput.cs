using TMPro;
using UnityEngine;

public class SizeInput : MonoBehaviour
{
    [SerializeField] private DebugLevel _debugLevel;
    [SerializeField] private TMP_InputField _text;

    void Awake()
    {
        _text.text = _debugLevel.CellSize.ToString();
    }
}
