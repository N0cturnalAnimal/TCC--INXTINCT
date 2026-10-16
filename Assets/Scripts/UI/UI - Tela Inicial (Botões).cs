
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void CheatMode()
    {
        SceneManager.LoadScene("UI - Cheat Mode");
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
