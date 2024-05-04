using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public Slider playerLevelBar;
    public GameObject LevelUpMenu;
    public TextMeshProUGUI DamageStat;
    public TextMeshProUGUI AgilityStat;
    public TextMeshProUGUI ArmorStat;
    private void Awake()
    {
        Instance = this;
    }
    public void IncreaseLevel(int inscrease)
    {
        PlayerStats.Instance.currentLevel += inscrease;
        playerLevelBar.value = PlayerStats.Instance.currentLevel;
        if (playerLevelBar.value == playerLevelBar.maxValue)
        {
            PlayerStats.Instance.currentLevel = 0;
            playerLevelBar.value = 0;
            LevelUpMenu.SetActive(true);
            DamageStat.text = PlayerStats.Instance.damage.ToString();
            AgilityStat.text = PlayerStats.Instance.agility.ToString();
            ArmorStat.text = PlayerStats.Instance.armor.ToString();
        }
    }
    public void IncreaseDamage()
    {
        PlayerStats.Instance.damage++;
        LevelUpMenu.SetActive(false);
    }
    public void IncreaseAgility()
    {
        PlayerStats.Instance.agility += 0.1f;
        LevelUpMenu.SetActive(false);
    }
    public void IncreaseArmor()
    {
        PlayerStats.Instance.armor++;
        LevelUpMenu.SetActive(false);
    }
}
