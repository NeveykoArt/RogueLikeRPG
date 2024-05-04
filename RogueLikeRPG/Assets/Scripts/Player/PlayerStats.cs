using System.Collections;
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
    public float agility { get; set; } = 2f;
    public float currentAgility { get; set; }

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
    public IEnumerable DamageDebuff(int debuff,  int delay)
    {
        currentDamage -= debuff;
        yield return new WaitForSeconds(delay);
        currentDamage = damage;
    }
    public IEnumerable AgilityDebuff(float debuff, int delay)
    {
        currentAgility -= debuff;
        yield return new WaitForSeconds(delay);
        currentAgility = agility;
    }
    public IEnumerable ArmorDebuff(int debuff, int delay)
    {
        currentArmor -= debuff;
        yield return new WaitForSeconds(delay);
        currentArmor = armor;
    }
}
