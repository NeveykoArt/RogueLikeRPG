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
    public GameObject shadow;

    private int health = 100;
    private int currentHealth;

    private Transform player;
    private float nextAttackTime = 0f;
    private float agroDistanceMax = 10f;
    private float agroDistanceMin = 1.5f;

    private float roamingDistanceMax = 4f;
    private float roamingDistanceMin = 2f;

    private float roamingTimerMax = 10f;
    private float roamingTime = 10f;
    private float idleTimerMax = 15f;
    private float idleTime = 15f;
    private bool idleFlag = true;

    private NavMeshAgent navMeshAgent;
    private Vector3 roamingPosition;
    private Vector3 startingPosition;

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
        if (Vector3.Distance(player.position, transform.position) < agroDistanceMax 
            && Vector3.Distance(player.position, transform.position) > agroDistanceMin)
        {
            enemyVisual.SetRunningAnimation(true);
            SetDestination(player.position);
        } else if (Vector3.Distance(player.position, transform.position) <= agroDistanceMin)
        {
            enemyVisual.SetRunningAnimation(false);
            navMeshAgent.SetDestination(transform.position);
            if (Time.time >= nextAttackTime)
            {
                string attack = gameObject.tag.ToString() + "Attack" + UnityEngine.Random.Range(1, 4).ToString();
                enemyVisual.SetAnimation(attack);
                nextAttackTime = Time.time + 1.25f;
            }
        } else
        {
            if (idleFlag)
            {
                idleTime -= Time.deltaTime;
                if (idleTime < 0)
                {
                    idleTime = idleTimerMax;
                    enemyVisual.SetRoamingAnimation(true);
                    SetDestination(GetRoamingPosition());
                    idleFlag = false;
                }
            } else
            {
                roamingTime -= Time.deltaTime;
                if (transform.position == roamingPosition || roamingTime < 0)
                {
                    roamingTime = roamingTimerMax;
                    enemyVisual.SetRoamingAnimation(false);
                    idleFlag = true;
                }
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
        enabled = false;
    }

    private void SetDestination(Vector3 position)
    {
        startingPosition = transform.position;
        roamingPosition = position;
        ChangeFacingDirection(startingPosition, roamingPosition);
        navMeshAgent.SetDestination(roamingPosition);
        startingPosition = transform.position;
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
