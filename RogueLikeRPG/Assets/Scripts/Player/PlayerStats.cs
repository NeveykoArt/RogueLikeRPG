using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; set; }
    public int health { get; set; } = 150;
    private int _currentHealth;
    public int currentHealth 
    {
        get => _currentHealth; 
        set
        {
            _currentHealth = value;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, health); 
        } 
    }
    public int level { get; private set; } = 100;
    public int currentLevel { get; set; }
    public int damage { get; set; } = 20;
    public int currentDamage { get; set; }
    public int armor { get; set; } = 10;
    public int currentArmor { get; set; }
    public int agility { get; set; } = 20;
    public int currentAgility { get; set; }
    public int points { get; set; } = 0;
    public int dungeons { get; set; } = 0;

    private void Awake()
    {
        Instance = this;
        currentHealth = health;
        currentDamage = damage;
        currentLevel = 0;
        currentArmor = armor;
        currentAgility = agility;
    }
    public void UpdateActiveStats()
    {
        currentDamage = damage;
        currentArmor = armor;
        currentAgility = agility;
    }
}
