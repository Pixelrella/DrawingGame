using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundEndAnimation : MonoBehaviour
{
    [SerializeField] private Round _round;
    [SerializeField] private List<GameObject> _visibleInRound = new List<GameObject>();
    [SerializeField] private List<GameObject> _hiddenInRound = new List<GameObject>();

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

        foreach (var item in _hiddenInRound)
        {
            item.SetActive(false);
        }

        foreach (var item in _visibleInRound)
        {
            item.SetActive(true);
        }
    }

    private void Play()
    {
        Camera.main.backgroundColor = Random.ColorHSV();

        foreach (var item in _hiddenInRound)
        {
            item.SetActive(true);
        }

        foreach (var item in _visibleInRound)
        {
            item.SetActive(false);
        }

        StartCoroutine(AnimatePixels());
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