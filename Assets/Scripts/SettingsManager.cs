using UnityEngine;
using UnityEngine.SceneManagement; // <-- IMPORTANTE: Adicionado aqui no topo

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    [Header("Configuração da UI")]
    public GameObject settingsCanvas;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (settingsCanvas != null)
            settingsCanvas.SetActive(false);
    }

    public void Open()
    {
        if (settingsCanvas != null)
            settingsCanvas.SetActive(true);

        if (PauseManager.Instance != null)
            PauseManager.Instance.CanTogglePause = false;
    }

    public void Close()
    {
        if (settingsCanvas != null)
            settingsCanvas.SetActive(false);

        if (PauseManager.Instance != null)
            PauseManager.Instance.CanTogglePause = true;
    }

    void Update()
    {
        // (Aquele opcional de fechar com Esc que você pode ter colocado)
        if (settingsCanvas != null && settingsCanvas.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Close();
        }
    }

    // =======================================================
    // BLOCO ADICIONADO (SOLUÇÃO 1) - Vai no final, antes da última chave
    // =======================================================
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
        // Sempre que mudar de cena, esconde as configurações
        if (settingsCanvas != null) 
            settingsCanvas.SetActive(false);
    }
}