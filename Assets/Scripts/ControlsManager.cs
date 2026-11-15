using UnityEngine;
using UnityEngine.SceneManagement; // <-- IMPORTANTE: Adicionado aqui no topo

public class ControlsManager : MonoBehaviour
{
    public static ControlsManager Instance { get; private set; }

    [Header("Configuração da UI")]
    public GameObject controlsCanvas;
    public GameObject settingsCanvas;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (controlsCanvas != null)
            controlsCanvas.SetActive(false);
    }

    public void OpenControls()
    {
        if (settingsCanvas != null) 
            settingsCanvas.SetActive(false); 
            
        if (controlsCanvas != null) 
            controlsCanvas.SetActive(true);  
    }

    public void BackToSettings()
    {
        if (controlsCanvas != null) 
            controlsCanvas.SetActive(false); 
            
        if (settingsCanvas != null) 
            settingsCanvas.SetActive(true);  
    }

    void Update()
    {
        if (controlsCanvas != null && controlsCanvas.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                BackToSettings();
            }
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
        // Sempre que mudar de cena, esconde os controles
        if (controlsCanvas != null) 
            controlsCanvas.SetActive(false);
    }
}