using UnityEngine;
using UnityEngine.SceneManagement;

public class UIscript : MonoBehaviour
{
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Settings()
    {
        SceneManager.LoadScene(1);
    }

    public void ChooseGame()
    {
        SceneManager.LoadScene(2);
    }

    public void PlayPointGame()
    {
        SceneManager.LoadScene(3);
        Time.timeScale = 1;
    }

    public void PlaySpeedRunGame()
    {
        SceneManager.LoadScene(4);
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
