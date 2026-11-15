using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    public Image fadeImage;
    public float fadeDuration = 0.5f;

    private void Awake()
    {
        Instance = this;

        fadeImage.color = new Color(0, 0, 0, 0);
    }

    public IEnumerator FadeOut()
{
    float elapsed = 0f;

    while (elapsed < fadeDuration)
    {
        elapsed += Time.deltaTime;

        float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);

        fadeImage.color = new Color(0, 0, 0, alpha);

        yield return null;
    }

    fadeImage.color = new Color(0, 0, 0, 1);
}

    public IEnumerator FadeIn()
{
    float elapsed = 0f;

    while (elapsed < fadeDuration)
    {
        elapsed += Time.deltaTime;

        float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);

        fadeImage.color = new Color(0, 0, 0, alpha);

        yield return null;
    }

    fadeImage.color = new Color(0, 0, 0, 0);
}
}