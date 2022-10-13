using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UndoButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        InputHandler.UndoAvailable += EnableButton;
    }

    private void EnableButton(bool flag)
    {
        _button.interactable = flag;
    }

    private void OnDestroy()
    {
        InputHandler.UndoAvailable -= EnableButton;
    }
}
