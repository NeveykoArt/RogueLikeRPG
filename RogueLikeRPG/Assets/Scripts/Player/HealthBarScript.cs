using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider healthbar;

    void Start()
    {
        healthbar.maxValue = PlayerStats.Instance.health;
    }

    void Update()
    {
        healthbar.value = PlayerStats.Instance.currentHealth;
    }
}
