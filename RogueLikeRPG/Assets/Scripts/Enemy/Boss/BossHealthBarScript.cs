using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarScript : MonoBehaviour
{
    public static BossHealthBarScript Instance { get; private set; }

    public Slider healthbar;
    public GameObject healthbarCanvas;

    public GameObject winMenu;

    private void Awake()
    {
        Instance = this;
    }

    public void SetActiveHealthBar(bool activeFlag)
    {
        healthbarCanvas.SetActive(activeFlag);
    }

    public void SetMaxHealth(int health)
    {
        healthbar.maxValue = health;
    }

    public void SetCurrentHealth(int currentHealth)
    {
        healthbar.value = currentHealth;
    }

    public void SetWinMenu()
    {
        GameInput.Instance.PauseGame();
        GameInput.Instance.otherMenu = true;
        GameInput.Instance.textAndBars.SetActive(false);
        winMenu.SetActive(true);
        if (winMenu.GetComponent<WinMenuScript>() != null)
        {
            winMenu.GetComponent<WinMenuScript>().PrintTime();
        }
    }
}
