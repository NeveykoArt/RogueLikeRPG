using System;
using System.Collections;
using System.Collections.Generic;
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

    private float roamingTimerMax = 10f;
    private float roamingTime = 10f;
    private float idleTimerMax = 5f;
    private float idleTime = 5f;
    private bool idleFlag = true;

    private NavMeshAgent navMeshAgent;
    private Vector3 roamingPosition;
    private Vector3 startingPosition;

    [Range(1, 10)] public float agroRadius;
    [Range(.1f, 3)] public float meleeRadius;
    [Range(.1f, 3)] public float attackRadius;
    public Transform attackPoint;
    public LayerMask playerLayer;

    private float nextAttack = 0f;

    public bool showGizmos;

    public Status mobStatus = Status.Chasing;

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
            var agroCollider = Physics2D.OverlapCircle(transform.position, agroRadius, playerLayer);
            if (agroCollider)
            {
                if (mobStatus == Status.Chasing)
                {
                    Chasing();
                }
            }
            else
            {
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
        if (Mathf.Abs(Vector3.Distance(roamingPosition, transform.position)) < 0.1f || roamingTime < 0)
        {
            navMeshAgent.ResetPath();
            roamingTime = roamingTimerMax;
            enemyVisual.SetRoamingAnimation(false);
            idleFlag = true;
        }
    }

    private void Chasing()
    {
        var meleeCollider = Physics2D.OverlapCircle(transform.position, meleeRadius, playerLayer);
        if (meleeCollider != null)
        {
            SetDestination(player.position);
            enemyVisual.SetRunningAnimation(false);
            navMeshAgent.speed = 1f;
            DoAttack();
        }
        else
        {
            SetDestination(player.position);
            enemyVisual.SetRoamingAnimation(false);
            enemyVisual.SetRunningAnimation(true);
            navMeshAgent.speed = 1.5f;
        }
    }

    private void DoAttack()
    {
        var attackCollider = Physics2D.OverlapCircle(attackPoint.position, attackRadius, playerLayer);
        if (attackCollider != null)
        {
            navMeshAgent.ResetPath();
            if (nextAttack <= Time.time)
            {
                string attack = gameObject.tag.ToString() + "Attack" + UnityEngine.Random.Range(1, 4).ToString();
                enemyVisual.SetAnimation(attack);
                nextAttack = Time.time + 1.25f;
            }
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
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth , 0, health);

        string hurt = gameObject.tag.ToString() + "Hurt";

        enemyVisual.SetRoamingAnimation(false);
        enemyVisual.SetRunningAnimation(false);
        enemyVisual.SetAnimation(hurt);
        navMeshAgent.ResetPath();

        EnemyHealthBarUpdate();

        if (currentHealth <= 0)
        {
            enemyCanvas.SetActive(false);
            EnemyDie();
        }
    }

    public void EnemyDie()
    {
        enemyAttack.enabled = false;
        enemyVisual.SetDeadAnimation();
        navMeshAgent.enabled = false;
        GetComponent<Collider2D>().enabled = false;
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
        return startingPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(roamingDistanceMin, roamingDistanceMax);
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
            Gizmos.DrawSphere(transform.position, agroRadius);
            Gizmos.DrawSphere(transform.position, meleeRadius);
            Gizmos.DrawSphere(attackPoint.position, attackRadius);
        }
    }
}
