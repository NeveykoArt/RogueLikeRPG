using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public static HealthBarScript Instance { get; private set; }

    public Slider healthbar;

    public GameObject lossMenu;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        healthbar.maxValue = PlayerStats.Instance.health;
    }

    void Update()
    {
        healthbar.value = PlayerStats.Instance.currentHealth;
    }

    public void SetLossMenu()
    {
        GameInput.Instance.PauseGame();
        GameInput.Instance.otherMenu = true;
        GameInput.Instance.textAndBars.SetActive(false);
        lossMenu.SetActive(true);
        if (lossMenu.GetComponent<LossMenuScript>() != null)
        {
            lossMenu.GetComponent<LossMenuScript>().PrintPoints();
        }
    }
}
