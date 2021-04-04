using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Image))]
public class FlashImage : MonoBehaviour
{
    [Range(0, 1)] [SerializeField] float _startingAlpha = 0;
    [SerializeField] float _secondsForOneFlash = 2f;
    public float SecondsForOneFlash
    {
        get { return _secondsForOneFlash; }
        private set
        {
            if (value < 0)
            {
                value = 0;
            }
            _secondsForOneFlash = value;
        }
    }

    [Range(0, 1)]
    [SerializeField] float _minAlpha = 0f;
    public float MinAlpha
    {
        get { return _minAlpha; }
        private set
        {
            _minAlpha = Mathf.Clamp(value, 0, 1);
        }
    }
    [Range(0, 1)]
    [SerializeField] float _maxAlpha = 1f;
    public float MaxAlpha
    {
        get { return _maxAlpha; }
        private set
        {
            _maxAlpha = Mathf.Clamp(value, 0, 1);
        }
    }

    // events
    public event Action OnStop = delegate { };
    public event Action OnCycleStart = delegate { };
    public event Action OnCycleComplete = delegate { };

    Coroutine _flashRoutine = null;
    Image _flashImage;

    private void Awake()
    {
        _flashImage = GetComponent<Image>();
        // initial state
        SetAlphaToDefault();
    }

    public void Flash()
    {
        if (_secondsForOneFlash <= 0) { return; }    // 0 speed wouldn't make sense

        if (_flashRoutine != null)
        {
            StopCoroutine(_flashRoutine);
        }
        _flashRoutine = StartCoroutine(Flash(SecondsForOneFlash, MinAlpha, MaxAlpha));
    }

    IEnumerator Flash(float secondsForOneFlash, float minAlpha, float maxAlpha)
    {

        // half the flash time should be on flash in, the other half for flash out
        float flashInDuration = secondsForOneFlash / 2;
        float flashOutDuration = secondsForOneFlash / 2;

        OnCycleStart.Invoke();
        // flash in
        for (float t = 0f; t <= flashInDuration; t += Time.deltaTime)
        {
            Color newColor = _flashImage.color;
            newColor.a = Mathf.Lerp(minAlpha, maxAlpha, t / flashInDuration);
            _flashImage.color = newColor;
            yield return null;
        }
        // flash out
        for (float t = 0f; t <= flashOutDuration; t += Time.deltaTime)
        {
            Color newColor = _flashImage.color;
            newColor.a = Mathf.Lerp(maxAlpha, minAlpha, t / flashOutDuration);
            _flashImage.color = newColor;
            yield return null;
        }

        OnCycleComplete.Invoke();
    }

    private void SetAlphaToDefault()
    {
        Color newColor = _flashImage.color;
        newColor.a = _startingAlpha;
        _flashImage.color = newColor;
    }

    private void SetNewFlashValues(float secondsForOneFlash, float minAlpha, float maxAlpha)
    {
        SecondsForOneFlash = secondsForOneFlash;
        MinAlpha = minAlpha;
        MaxAlpha = maxAlpha;
    }

}