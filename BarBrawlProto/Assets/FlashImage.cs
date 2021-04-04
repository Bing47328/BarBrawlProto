using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class FlashImage : MonoBehaviour
{
    Image _image = null;
    Coroutine _currentFlash = null;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void StartFlash(float seconds, float maxAlpha, Color newColor)
    {
        _image.color = newColor;

        maxAlpha = Mathf.Clamp(maxAlpha, 0, 1);

        if (_currentFlash != null)
        {
            StopCoroutine(_currentFlash);
            _currentFlash = StartCoroutine(Flash(seconds, maxAlpha));
        }
    }

    IEnumerator Flash(float seconds, float maxAlpha)
    {
        float flashInDuration = seconds / 2;
        for (float i = 0; i <= flashInDuration; i+= Time.deltaTime)
        {
            Color colorThisFrame = _image.color;
            colorThisFrame.a = Mathf.Lerp(0, maxAlpha, i / flashInDuration);
            _image.color = colorThisFrame;

            yield return null;
        }
    }
}
