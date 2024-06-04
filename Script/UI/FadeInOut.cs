using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    public Color fadeInColor = Color.black;
    public Color fadeOutColor = new Color(0, 0, 0, 0);

    public IEnumerator FadeIn(Image fadeImage)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = Color.Lerp(fadeInColor, fadeOutColor, t);
            yield return null;
        }
    }

    public IEnumerator FadeOut(Image fadeImage)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = Color.Lerp(fadeOutColor, fadeInColor, t);
            yield return null;
        }
    }
}
