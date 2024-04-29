using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIscript : MonoBehaviour
{
    public void Settings()
    {
        SceneManager.LoadScene(1);
    }

    public void ChooseGame()
    {
        SceneManager.LoadScene(2);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(3);
    }
}
