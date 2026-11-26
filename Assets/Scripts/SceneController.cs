
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    [Header("Áudio")]
    [Tooltip("Arraste aqui o AudioSource da música do Menu Principal")]
    [SerializeField] AudioSource menuMusic;

    private bool isLoading = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainMenu" && menuMusic != null)
        {
            menuMusic.Stop();
        }
    }

    public void NextLevel()
    {
        if (isLoading)
            return;

        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        isLoading = true;

        if (FadeManager.Instance != null)
        {
            yield return StartCoroutine(FadeManager.Instance.FadeOut());
        }
        else
        {
            Debug.LogError("FadeManager não encontrado!");
            isLoading = false;
            yield break;
        }

        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;

        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene);

        yield return operation;

        yield return StartCoroutine(FadeManager.Instance.FadeIn());

        isLoading = false;
    }
}
