using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    public Image fadeImage;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator FadeOut()
    {
        float time = 0;

        while(time < 1)
        {
            time += Time.deltaTime;
            fadeImage.color =
                new Color(0,0,0,time);

            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        float time = 1;

        while(time > 0)
        {
            time -= Time.deltaTime;
            fadeImage.color =
                new Color(0,0,0,time);

            yield return null;
        }
    }
}
