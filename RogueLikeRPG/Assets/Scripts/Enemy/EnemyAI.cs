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
    public LayerMask playerLayer;

    public float nextAttack = 0f;

    public bool showGizmos;

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
        var agroCollider = Physics2D.OverlapCircle(transform.position, agroRadius, playerLayer);
        var meleeCollider = Physics2D.OverlapCircle(transform.position, meleeRadius, playerLayer);
        if (agroCollider != null && meleeCollider == null)
        {
            enemyVisual.SetRunningAnimation(true);
            SetDestination(player.position);
            navMeshAgent.speed = 2.0f;
        } else if (agroCollider != null && meleeCollider != null)
        {
            enemyVisual.SetRunningAnimation(false);
            navMeshAgent.speed = 1.0f;
            navMeshAgent.ResetPath();
            if (nextAttack <= Time.time)
            {
                string attack = gameObject.tag.ToString() + "Attack" + UnityEngine.Random.Range(1, 4).ToString();
                enemyVisual.SetAnimation(attack);
                nextAttack = Time.time + 1.25f;
            }
        } else if (agroCollider == null && meleeCollider == null)
        {
            navMeshAgent.speed = 1.0f;
            if (idleFlag)
            {
                navMeshAgent.ResetPath();
                idleTime -= Time.deltaTime;
                if (idleTime < 0)
                {
                    idleTime = idleTimerMax;
                    enemyVisual.SetRoamingAnimation(true);
                    SetDestination(GetRoamingPosition());
                    idleFlag = false;
                }
            }
            else
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
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.DrawSphere(transform.position, agroRadius);
            Gizmos.DrawSphere(transform.position, meleeRadius);
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
        enemyVisual.SetAnimation(hurt);

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
}
