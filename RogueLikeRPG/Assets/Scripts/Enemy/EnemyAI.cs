using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Game.Utils;
using TMPro;

public class EnemyAI : MonoBehaviour
{
    public EnemyVisual enemyVisual;
    public EnemyAttack enemyAttack;

    public GameObject enemyCanvas;
    public Slider enemySlider;
    public TextMeshProUGUI enemyText;

    public GameObject projectile;

    public int health = 100;
    private int currentHealth;

    private Transform player;

    private float roamingDistanceMax = 3f;
    private float roamingDistanceMin = 1f;
    public float archerStepDistance = 0.5f;

    private float roamingTimerMax = 10f;
    private float roamingTime = 10f;
    private float idleTimerMax = 5f;
    private float idleTime = 5f;
    private bool idleFlag = true;
    private bool runFlag = false;
    private bool dodgeFlag = false;

    private NavMeshAgent navMeshAgent;
    private Vector3 roamingPosition;
    private Vector3 startingPosition;

    [Range(1, 10)] public float agroRadius;
    [Range(.1f, 3)] public float meleeRadius;
    [Range(.1f, 3)] public float offset;
    public LayerMask playerLayer;

    private float nextAttack = 0f;
    private float nextShoot = 0f;
    private float nextDodge = 0f;

    public bool showGizmos;

    public Status mobStatus = Status.Chasing;

    [Range(2, 3)] public int attackCount = 3;

    private Vector3 lastVector;

    public enum Status{
        Chasing,
        Shooting
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    private void Start()
    {
        currentHealth = health;
        player = Player.Instance.transform;
        startingPosition = transform.position;
        enemySlider.maxValue = health;
    }

    private void FixedUpdate()
    {
        if (!enemyVisual.IsHurt())
        {
            var agroCollider = Physics2D.OverlapCircle(new Vector3(transform.position.x, transform.position.y + offset, transform.position.z), agroRadius, playerLayer);
            if (agroCollider != null)
            {
                if (mobStatus == Status.Chasing)
                {
                    Chasing();
                } else if (mobStatus == Status.Shooting)
                {
                    Shooting();
                }
            }
            else
            {
                if (runFlag)
                {
                    navMeshAgent.ResetPath();
                    runFlag = false;
                    enemyVisual.SetRunningAnimation(runFlag);
                }
                if (idleFlag)
                {
                    Idling();
                }
                else
                {
                    Roaming();
                }
            }
        }
    }

    private void Idling()
    {
        navMeshAgent.ResetPath();
        idleTime -= Time.deltaTime;
        if (idleTime < 0)
        {
            idleTime = idleTimerMax;
            enemyVisual.SetRoamingAnimation(true);
            navMeshAgent.speed = 1.0f;
            SetDestination(GetRoamingPosition());
            idleFlag = false;
        }
    }

    private void Roaming()
    {
        roamingTime -= Time.deltaTime;
        if (Mathf.Abs(Vector3.Distance(roamingPosition, transform.position)) < 0.5f || roamingTime < 0)
        {
            navMeshAgent.ResetPath();
            roamingTime = roamingTimerMax;
            enemyVisual.SetRoamingAnimation(false);
            idleFlag = true;
        }
    }

    private void Chasing()
    {
        var meleeCollider = Physics2D.OverlapCircle(new Vector3(transform.position.x, transform.position.y + offset, transform.position.z), meleeRadius, playerLayer);
        if (meleeCollider != null)
        {
            navMeshAgent.ResetPath();
            runFlag = false;
            enemyVisual.SetRunningAnimation(runFlag);
            navMeshAgent.speed = 1f;
            DoAttack();
        }
        else
        {
            SetDestination(player.position);
            enemyVisual.SetRoamingAnimation(false);
            runFlag = true;
            enemyVisual.SetRunningAnimation(runFlag);
            navMeshAgent.speed = 1.5f;
        }
    }

    private void Shooting()
    {
        var meleeCollider = Physics2D.OverlapCircle(new Vector3(transform.position.x, transform.position.y + offset, transform.position.z), meleeRadius, playerLayer);
        if (meleeCollider != null)
        {
            ChangeFacingDirection(transform.position, player.position);
            DoAttack();
        }
        else
        {
            if (dodgeFlag)
            {
                Dodge();
            } else
            {
                ChangeFacingDirection(transform.position, player.position);
                DoShoot();
            }
        }
    }

    private void Dodge()
    {
        if (nextDodge <= Time.time)
        {
            navMeshAgent.speed = 3.0f;
            if (GetArcherPosition() != transform.position)
            {
                SetDestination(GetArcherPosition()); //делает шаг назад/вперед после выстрела противоположно текущему щагу игрока
            } else
            {
                SetDestination(transform.position + lastVector); //делает шаг назад/вперед после выстрела противоположно последнему щагу игрока
            }
            enemyVisual.SetAnimation(gameObject.tag.ToString() + "Dodge");
            dodgeFlag = false;
            nextDodge = Time.time + 2.65f;
        }
    }

    private void DoShoot()
    {
        if (nextShoot <= Time.time)
        {
            string shooting = gameObject.tag.ToString() + "Shot" + UnityEngine.Random.Range(1, 3).ToString();
            enemyVisual.SetAnimation(shooting);
            nextShoot = Time.time + 1.30f;
            if (nextDodge <= Time.time)
            {
                dodgeFlag = true;
            }
        }
    }

    private void DoAttack()
    {
        if (nextAttack <= Time.time)
        {
            string attack = gameObject.tag.ToString() + "Attack" + UnityEngine.Random.Range(1, attackCount).ToString();
            enemyVisual.SetAnimation(attack);
            nextAttack = Time.time + 1.25f;
        }
    }

    public void EnemyHealthBarUpdate()
    {
        if (enemyCanvas.activeSelf == false)
        {
            enemyCanvas.SetActive(true);
        }
        enemySlider.value = currentHealth;
        enemyText.text = currentHealth.ToString() + " / " + health.ToString();
    }

    public void EnemyTakeDamage(int damage)
    {
        navMeshAgent.ResetPath();
        enemyVisual.SetRoamingAnimation(false);
        enemyVisual.SetRunningAnimation(false);

        string hurt = gameObject.tag.ToString() + "Hurt";
        enemyVisual.SetAnimation(hurt);

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth , 0, health);

        EnemyHealthBarUpdate();

        if (currentHealth <= 0)
        {
            enemyCanvas.SetActive(false);
            EnemyDie();
        }
    }

    public void EnemyDie()
    {
        GetComponent<Collider2D>().enabled = false;
        enemyAttack.enabled = false;
        enemyVisual.SetDeadAnimation();
        navMeshAgent.ResetPath();
        navMeshAgent.enabled = false;
        Instantiate(projectile, transform.position, Quaternion.identity);
        enabled = false;
    }

    private void SetDestination(Vector3 position)
    {
        startingPosition = transform.position;
        roamingPosition = position;
        ChangeFacingDirection(startingPosition, roamingPosition);
        navMeshAgent.SetDestination(roamingPosition);
    }

    private Vector3 GetRoamingPosition()
    {
        return transform.position + Utils.GetRandomDir() * UnityEngine.Random.Range(roamingDistanceMin, roamingDistanceMax);
    }

    private Vector3 GetArcherPosition()
    {
        lastVector = (new Vector3(GameInput.Instance.GetMovementVector().x, GameInput.Instance.GetMovementVector().y) * archerStepDistance);
        return transform.position + (new Vector3(GameInput.Instance.GetMovementVector().x, GameInput.Instance.GetMovementVector().y) * archerStepDistance);
    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (sourcePosition.x > targetPosition.x)
        {
            enemyVisual.ChangeFasing(Quaternion.Euler(0, -180, 0));
        } else
        {
            enemyVisual.ChangeFasing(Quaternion.Euler(0, 0, 0));
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
