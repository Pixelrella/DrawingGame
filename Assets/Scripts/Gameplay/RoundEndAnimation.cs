using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RoundEndAnimation : MonoBehaviour
{
    [SerializeField] private Round _round;
    [SerializeField] private RectTransform _restartPanel;

    private Pixel[] _pixels;
    private Color _cameraColor;
    private Vector2 _restartPanelPos;
    private Vector2 _restartPanelAnchorMax;
    private Vector2 _restartPanelAnchorMin;
    private LayoutGroup _restartPanelLayoutParent;

    private void Awake()
    {
        _cameraColor = Camera.main.backgroundColor;

        _restartPanelPos = _restartPanel.anchoredPosition;
        _restartPanelAnchorMax = _restartPanel.anchorMax;
        _restartPanelAnchorMin = _restartPanel.anchorMin;
        _restartPanelLayoutParent = _restartPanel.transform.parent.GetComponent<LayoutGroup>();

        _round.RoundEnd += Play;
        _round.RoundStart += Init;
    }

    public void Init()
    {
        Camera.main.backgroundColor = _cameraColor;
        _pixels = FindObjectsOfType<Pixel>();

        _restartPanel.anchoredPosition = _restartPanelPos;
        _restartPanel.anchorMax = _restartPanelAnchorMax;
        _restartPanel.anchorMin = _restartPanelAnchorMin;
        _restartPanelLayoutParent.enabled = true;
    }

    private void Play()
    {
        Camera.main.backgroundColor = Random.ColorHSV();
        StartCoroutine(AnimatePixels());
        StartCoroutine(ShowRestartPanel());
    }

    private IEnumerator ShowRestartPanel()
    {
        yield return new WaitForSeconds(3f);
        _restartPanelLayoutParent.enabled = false;
        _restartPanel.anchorMax = new Vector2(.5f, .5f);
        _restartPanel.anchorMin = new Vector2(.5f, .5f);
        _restartPanel.anchoredPosition = Vector2.zero;
    }

    private IEnumerator AnimatePixels()
    {
        yield return new WaitForSeconds(.5f);

        foreach (var pixel in _pixels)
        {
            if (pixel == null)
            { //TODO find out why one pixel in next round is null
                continue;
            }

            pixel.GetComponent<Collider2D>().isTrigger = false;
            pixel.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void OnDestroy()
    {
        _round.RoundEnd -= Play;
        _round.RoundStart -= Init;
    }
}