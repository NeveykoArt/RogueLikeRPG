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

    public GameObject enemyCanvas;
    public Slider enemySlider;
    public TextMeshProUGUI enemyText;
    public GameObject shadow;

    private int health = 100;
    private int currentHealth;

    private int damage = 10;
    public Transform attackPoint;
    public float attackRange = 2f;
    private float nextAttackTime = 0f;
    public LayerMask playerLayer;

    private float roamingDistanceMax = 5f;
    private float roamingDistanceMin = 3f;

    private float roamingTimerMax = 5f;
    private float roamingTime = 10f;
    private float idleTimerMax = 5f;
    private float idleTime = 5f;

    private State startingState;
    private NavMeshAgent navMeshAgent;
    private State commonState;
    private Vector3 roamingPosition;
    private Vector3 startingPosition;

    private enum State
    {
        Idle,
        Roaming,
        Dead,
        Attack
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        commonState = startingState;
    }

    private void Start()
    {
        currentHealth = health;
    }

    private void FixedUpdate()
    {
        var player = Player.Instance.transform;
        if (Vector3.Distance(player.position, transform.position) < 10f)
        {
            enemyVisual.SetRoamingAnimation(true);
            ChangeFacingDirection(transform.position, player.position);
            navMeshAgent.SetDestination(player.position);
            commonState = State.Attack;
        }
        switch (commonState)
        {
            default:
            case State.Idle:
                idleTime -= Time.deltaTime;
                if (idleTime < 0)
                {
                    idleTime = idleTimerMax;
                    enemyVisual.SetRoamingAnimation(true);
                    Roaming();
                    commonState = State.Roaming;
                }
                break;
            case State.Roaming:
                roamingTime -= Time.deltaTime;
                if (transform.position == roamingPosition || roamingTime < 0)
                {
                    roamingTime = roamingTimerMax;
                    enemyVisual.SetRoamingAnimation(false);
                    commonState = State.Idle;
                }
                break;
            case State.Dead:
                navMeshAgent.enabled = false;
                GetComponent<Collider2D>().enabled = false;
                enabled = false;
                break;
            case State.Attack:
                if (Vector3.Distance(player.position, transform.position) <= 3f && Time.time >= nextAttackTime)
                {
                    enemyVisual.SetRoamingAnimation(false);
                    string attack = gameObject.tag.ToString() + "Attack" + UnityEngine.Random.Range(1, 4).ToString();
                    enemyVisual.SetAttackingAnimation(attack);
                    Attack();
                    nextAttackTime = Time.time + 1.25f;
                }
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth , 0, health);

        enemyCanvas.SetActive(true);
        enemySlider.value = currentHealth;
        enemyText.text = currentHealth.ToString() + " / " + health.ToString();

        if (currentHealth <= 0)
        {
            commonState = State.Dead;
            enemyCanvas.SetActive(false);
            shadow.SetActive(false);
            Die();
        }
    }

    public void Die()
    {
        enemyVisual.SetDeadAnimation();
    }

    public void Attack()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        if (hitPlayer != null)
        {
            Player.Instance.TakeDamage(damage);
        }
    }

    private void Roaming()
    {
        startingPosition = transform.position;
        roamingPosition = GetRoamingPosition();
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
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
