using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RoundEndAnimation : MonoBehaviour
{
    [SerializeField] private Round _round;
    [SerializeField] private GameObject _restartPanel;

    private Pixel[] _pixels;
    private Color _originalCameraColor;

    private void Awake()
    {
        _originalCameraColor = Camera.main.backgroundColor;
        _round.RoundEnd += Play;
        _round.RoundStart += Init;
    }

    public void Init()
    {
        Camera.main.backgroundColor = _originalCameraColor;
        _pixels = FindObjectsOfType<Pixel>();
        _restartPanel.SetActive(false);
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

        _restartPanel.SetActive(true);
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