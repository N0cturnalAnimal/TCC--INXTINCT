using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    // ============================================================
    // SINGLETON — garante que só exista UM PauseManager no jogo
    // ============================================================
    public static PauseManager Instance { get; private set; }

    [Header("Configuração da UI")]
    [Tooltip("Arraste aqui a Canvas do menu de pause (deve ser filha deste objeto)")]
    public GameObject pauseMenuCanvas;

    // Estado atual do jogo
    public bool IsPaused { get; private set; } = false;

    // Flag para bloquear o Esc quando outra UI já está usando (ex: Configurações abertas)
    public bool CanTogglePause { get; set; } = true;

    // ============================================================
    // AWAKE — configura o Singleton e a persistência entre cenas
    // ============================================================
    void Awake()
    {
        // Se já existe outro PauseManager, destrói ESTE (a cópia duplicada)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Este é o PauseManager oficial
        Instance = this;

        // Faz este objeto (e todos os filhos, inclusive a Canvas) sobreviverem
        // à troca de Scene
        DontDestroyOnLoad(gameObject);
    }

    // ============================================================
    // UPDATE — detecta o Esc para pausar/despausar
    // ============================================================
    void Update()
    {
        // Se outra UI bloqueou o pause, ignora o Esc
        if (!CanTogglePause) return;

        // GetKeyDown = dispara UMA vez por pressionamento (não a cada frame)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // ============================================================
    // MÉTODOS PÚBLICOS — podem ser chamados por botões da UI
    // ============================================================
    public void TogglePause()
    {
        if (IsPaused) ResumeGame();
        else PauseGame();
    }

    public void PauseGame()
    {
        if (pauseMenuCanvas != null)
            pauseMenuCanvas.SetActive(true);

        Time.timeScale = 0f;   // Congela o tempo do jogo
        IsPaused = true;
    }

    public void ResumeGame()
    {
        if (pauseMenuCanvas != null)
            pauseMenuCanvas.SetActive(false);

        Time.timeScale = 1f;   // Restaura o tempo
        IsPaused = false;
    }

    // ============================================================
    // Troca de cena com reset de tempo (para botões "Menu Principal" etc.)
    // ============================================================
    public void LoadSceneAndResetTime(string sceneName)
    {
        Time.timeScale = 1f;   // MUITO IMPORTANTE: reseta antes de trocar
        IsPaused = false;

        if (pauseMenuCanvas != null)
            pauseMenuCanvas.SetActive(false);

        SceneManager.LoadScene(sceneName);
    }
}