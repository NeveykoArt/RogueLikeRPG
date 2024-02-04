using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public PlayerVisual playerVisual;

    private Rigidbody2D rb;
    [SerializeField] private float movingSpeed = 5f;
    private float minMovingSpeed = 0.1f;
    private bool isWalking = false;
    private Vector2 inputVector;

    private int health = 150;
    private int currentHealth;

    private int damage = 10;
    public Transform attackPoint;
    public float attackRange = 2f;
    public float attackRate = 1f;
    private float nextAttackTime = 0f;
    public LayerMask enemyLayers;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
        currentHealth = health;
    }

    private void GameInput_OnPlayerAttack(object sender, System.EventArgs e)
    {
        if (Time.time >= nextAttackTime)
        {
            playerVisual.SetCombatAnimation();
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyAI>().TakeDamage(damage);
            }
            nextAttackTime = Time.time + 1.25f / attackRate;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, health);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        playerVisual.SetDieAnimation();
    }

    private void Update()
    {
        inputVector = GameInput.Instance.GetMovementVector();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.fixedDeltaTime));
        if (Mathf.Abs(inputVector.x) > minMovingSpeed || Mathf.Abs(inputVector.y) > minMovingSpeed)
        {
            isWalking = true;
        } else
        {
            isWalking = false;
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
