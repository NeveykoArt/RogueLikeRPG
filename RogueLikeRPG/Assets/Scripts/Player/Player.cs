using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public PlayerVisual playerVisual;

    private Rigidbody2D rb;
    private float minMovingSpeed = 0.1f;
    public bool isWalking { get; private set; } = false;
    private Vector2 inputVector;

    public float nextAttack = 0f;

    public Slider playerHealthBar;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
        playerHealthBar.maxValue = PlayerStats.Instance.health;
    }

    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e)
    {
        if (nextAttack <= Time.time)
        {
            string attack = "Attack" + UnityEngine.Random.Range(1, 4).ToString();
            playerVisual.SetAnimation(attack);
            nextAttack = Time.time + 1f;
        }
    }

    public void TakeDamage(int damage)
    {
        PlayerStats.Instance.currentHealth -= damage - PlayerStats.Instance.currentArmor / 2;
        PlayerStats.Instance.currentHealth = Mathf.Clamp(PlayerStats.Instance.currentHealth, 0, PlayerStats.Instance.health);

        playerVisual.SetAnimation("Hurt");

        playerHealthBar.value = PlayerStats.Instance.currentHealth;

        if (PlayerStats.Instance.currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        playerVisual.SetDieAnimation();
        GetComponent<Collider2D>().enabled = false;
        enabled = false;
    }

    private void Update()
    {
        inputVector = GameInput.Instance.GetMovementVector();
    }

    private void FixedUpdate()
    {
        if (!playerVisual.IsHurt()) {
            HandleMovement();
        }
        PlayerStats.Instance.UpdateActiveStats();
    }

    private void HandleMovement()
    {
        rb.MovePosition(rb.position + inputVector * (PlayerStats.Instance.currentAgility * Time.fixedDeltaTime));
        if (Mathf.Abs(inputVector.x) > minMovingSpeed || Mathf.Abs(inputVector.y) > minMovingSpeed)
        {
            isWalking = true;
        } else
        {
            isWalking = false;
        }
    }

    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }
}
