using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    
    [Header("UI / Transição")]
    [SerializeField] Animator transitionAnim;

    [Header("Áudio")]
    [Tooltip("Arraste aqui o AudioSource da música do Menu Principal")]
    [SerializeField] AudioSource menuMusic; 

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- SISTEMA DE SEGURANÇA DE ÁUDIO ---
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
        // Se a cena que acabou de carregar NÃO for a Tela Inicial (MainMenu),
        // nós forçamos a música do menu a parar.
        // (Troque "MainMenu" pelo nome exato da sua cena inicial)
        if (scene.name != "MainMenu" && menuMusic != null)
        {
            menuMusic.Stop();
        }
    }
    // -------------------------------------

    public void NextLevel()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        if (transitionAnim != null)
            transitionAnim.SetTrigger("End");

        yield return new WaitForSeconds(1);

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        if (transitionAnim != null)
            transitionAnim.SetTrigger("Start");
    }
}