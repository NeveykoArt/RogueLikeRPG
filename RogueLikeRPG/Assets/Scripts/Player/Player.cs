using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public PlayerVisual playerVisual;

    private Rigidbody2D rb;
    private float minMovingSpeed = 0.1f;

    public bool isWalking { get; private set; } = false;
    public bool isProtect { get; private set; } = false;

    private Vector2 inputVector;

    public float nextAttack = 0f;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
    }

    private void Update()
    {
        inputVector = GameInput.Instance.GetMovementVector();
    }

    private void FixedUpdate()
    {
        if (!playerVisual.isHurt()) {
            if (GameInput.Instance.ProtectStatus())
            {
                isWalking = false;
                isProtect = true;
            }
            else
            {
                isProtect = false;
                HandleMovement();
            }
        }
        PlayerStats.Instance.UpdateActiveStats();
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
        if (!isProtect) {
            PlayerStats.Instance.currentHealth -= damage - PlayerStats.Instance.currentArmor / 2;

            playerVisual.SetAnimation("Hurt");

            if (PlayerStats.Instance.currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void Heal(int healnumber)
    {
        PlayerStats.Instance.currentHealth += healnumber;
    }

    public void Die()
    {
        playerVisual.SetDieAnimation();
        playerVisual.GetComponent<ShadowCaster2D>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GameInput.Instance.OnPlayerAttack -= GameInput_OnPlayerAttack;
        enabled = false;
    }

    private void HandleMovement()
    {
        if (Input.GetKey("space") && playerVisual.dashFlag)
        {
            rb.MovePosition(rb.position + inputVector * (((float)PlayerStats.Instance.currentAgility / 10) * Time.fixedDeltaTime * 2));
            playerVisual.SetAnimation("Dash");
        }
        else
        {
            rb.MovePosition(rb.position + inputVector * (((float)PlayerStats.Instance.currentAgility / 10) * Time.fixedDeltaTime));
            if (Mathf.Abs(inputVector.x) > minMovingSpeed || Mathf.Abs(inputVector.y) > minMovingSpeed)
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }
        }
    }

    public Vector3 GetPlayerScreenPosition()
    {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }
}
