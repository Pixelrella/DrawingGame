using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorPalette", menuName = "ScriptableObjects/ColorSetupScriptableObject", order = 1)]
public class ColorPaletteScriptableObject : ScriptableObject
{
    [SerializeField] private List<Color> _colors;

    public List<Color> Colors => _colors;
}