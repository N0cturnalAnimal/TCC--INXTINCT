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
        // 1. Proteção Singleton: só permite um FadeManager no jogo
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // 2. Faz o Fade sobreviver à troca de cenas
        DontDestroyOnLoad(gameObject);

        // Garante que começa transparente
        fadeImage.color = new Color(0, 0, 0, 0);
    }

    public IEnumerator FadeOut()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            // 3. O SEGREDO ESTÁ AQUI: unscaledDeltaTime roda mesmo se o jogo estiver pausado
            elapsed += Time.unscaledDeltaTime;

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
            // O mesmo segredo aqui
            elapsed += Time.unscaledDeltaTime;

            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);

            fadeImage.color = new Color(0, 0, 0, alpha);

            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 0);
    }
}