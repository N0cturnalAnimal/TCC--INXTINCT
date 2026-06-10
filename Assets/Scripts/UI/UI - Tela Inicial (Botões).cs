using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Fase1e2");
    }

    public void Controls()
    {
        SceneManager.LoadScene("UI - Controles");
    }

    public void Settings()
    {
        SceneManager.LoadScene("UI - Configurações");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Back()
    {
        SceneManager.LoadScene("UI - Tela Inicial");
    }
}