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

    public GameObject DamageButton;
    public GameObject AgilityButton;
    public GameObject ArmorButton;

    private int newLevels = 0;

    private void Awake()
    {
        Instance = this;
        newLevels = 0;
    }
    private void Update()
    {
        if (newLevels == 0)
        {
            LevelUpMenu.SetActive(false);
        }
    }
    public void IncreaseLevel(int inscrease)
    {
        PlayerStats.Instance.currentLevel += inscrease;
        playerLevelBar.value = PlayerStats.Instance.currentLevel;
        if (playerLevelBar.value == playerLevelBar.maxValue)
        {
            newLevels += 1;
            PlayerStats.Instance.currentLevel = 0;
            playerLevelBar.value = 0;
            LevelUpMenu.SetActive(true);
            DamageStat.text = PlayerStats.Instance.currentDamage.ToString();
            if (PlayerStats.Instance.currentDamage < 40)
            {
                DamageButton.SetActive(true);
            }
            AgilityStat.text = ((float)PlayerStats.Instance.currentAgility / 10).ToString();
            if (PlayerStats.Instance.currentAgility < 30)
            {
                AgilityButton.SetActive(true);
            }
            ArmorStat.text = PlayerStats.Instance.currentArmor.ToString();
            if (PlayerStats.Instance.currentArmor < 40)
            {
                ArmorButton.SetActive(true);
            }
        }
    }
    public void IncreaseDamage()
    {
        PlayerStats.Instance.currentDamage++;
        newLevels -= 1;
        DamageStat.text = PlayerStats.Instance.currentDamage.ToString();
        if (PlayerStats.Instance.currentDamage >= 40)
        {
            DamageButton.SetActive(false);
        }
    }
    public void IncreaseAgility()
    {
        PlayerStats.Instance.currentAgility += 1;
        newLevels -= 1;
        AgilityStat.text = ((float)PlayerStats.Instance.currentAgility / 10).ToString();
        if (PlayerStats.Instance.currentAgility >= 30)
        {
            AgilityButton.SetActive(false);
        }
    }
    public void IncreaseArmor()
    {
        PlayerStats.Instance.currentArmor++;
        newLevels -= 1;
        ArmorStat.text = PlayerStats.Instance.currentArmor.ToString();
        if (PlayerStats.Instance.currentArmor >= 40)
        {
            ArmorButton.SetActive(false);
        }
    }
}
