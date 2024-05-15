using System;
using UnityEngine;
using Pathfinding;

public class BossAI : MonoBehaviour
{
    public BossVisual bossVisual;
    public BossAttack bossAttack;

    public GameObject projectile;

    public bool showGizmos;

    public LayerMask playerLayer;

    [Range(1, 15)] public float agroRadius;
    [Range(.1f, 3)] public float meleeRadius;
    [Range(.1f, 3)] public float offset;

    public int health = 100;
    public int currentHealth;

    private Transform player;

    private float bossStepDistance = 1f;
    private bool walkingFlag = false;

    private AIPath aiPath;
    private Vector3 roamingPosition;
    private Vector3 startingPosition;

    private float nextAttack = 0f;
    private float nextShoot = 0f;
    private float nextWalk = 0f;

    private void Awake()
    {
        aiPath = GetComponent<AIPath>();
    }

    private void Start()
    {
        currentHealth = health;
        player = Player.Instance.transform;
        startingPosition = transform.position;
        BossHealthBarScript.Instance.SetActiveHealthBar(true);
        BossHealthBarScript.Instance.SetMaxHealth(health);
    }

    private void FixedUpdate()
    {
        var agroCollider = Physics2D.OverlapCircle(new Vector3(transform.position.x, transform.position.y + offset, transform.position.z), agroRadius, playerLayer);
        if (agroCollider != null)
        {
            ChangeFacingDirection(transform.position, player.position);
            Shooting();
        }
    }

    private void Shooting()
    {
        var meleeCollider = Physics2D.OverlapCircle(new Vector3(transform.position.x, transform.position.y + offset, transform.position.z), meleeRadius, playerLayer);
        if(walkingFlag)
        {
            WalkToPlayer();
        } 
        else
        {
            if (meleeCollider != null)
            {
                walkingFlag = false;
                DoAttack();
            } 
            else
            {
                DoShoot();
            }
        }
    }

    private void WalkToPlayer()
    {
        if (nextWalk <= Time.time)
        {
            aiPath.maxSpeed = 2f;
            SetDestination(GetChasePosition());
            bossVisual.SetChasingAnimation();
            walkingFlag = false;
            nextWalk = Time.time + 2.65f;
        }
    }

    private void DoShoot()
    {
        if (nextShoot <= Time.time)
        {
            string shooting = "BossAttack3";
            bossVisual.SetAnimation(shooting);
            nextShoot = Time.time + 1.30f;
            if (nextWalk <= Time.time)
            {
                walkingFlag = true;
            }
        }
    }

    private void DoAttack()
    {
        if (nextAttack <= Time.time)
        {
            string attack = "BossAttack" + UnityEngine.Random.Range(1, 3).ToString();
            bossVisual.SetAnimation(attack);
            nextAttack = Time.time + 1.25f;
        }
    }

    public void BossTakeDamage(int damage)
    {
        aiPath.SetPath(null);
        bossVisual.SetWalkingAnimation(false);

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, health);

        BossHealthBarScript.Instance.SetCurrentHealth(currentHealth);

        if (currentHealth <= 0)
        {
            BossDie();
        }
    }

    private void BossDie()
    {
        aiPath.SetPath(null);
        aiPath.enabled = false;
        BossHealthBarScript.Instance.SetActiveHealthBar(false);
        bossAttack.enabled = false;
        bossVisual.SetDeadAnimation();
        enabled = false;
    }

    private void SetDestination(Vector3 position)
    {
        startingPosition = transform.position;
        roamingPosition = position;
        ChangeFacingDirection(startingPosition, roamingPosition);
        aiPath.destination = roamingPosition;
    }

    private Vector3 GetChasePosition()
    {
        return transform.position + ((Player.Instance.transform.position - transform.position).normalized * bossStepDistance);
    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (sourcePosition.x > targetPosition.x)
        {
            bossVisual.ChangeFasing(Quaternion.Euler(0, -180, 0));
        }
        else
        {
            bossVisual.ChangeFasing(Quaternion.Euler(0, 0, 0));
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + offset, transform.position.z), agroRadius);
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + offset, transform.position.z), meleeRadius);
        }
    }
}
