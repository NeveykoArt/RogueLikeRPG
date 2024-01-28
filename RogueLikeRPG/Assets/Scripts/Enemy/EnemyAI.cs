using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Utils;

public class EnemyAI : MonoBehaviour
{
    public EnemyVisual enemyVisual;

    [SerializeField] private State startingState;
    [SerializeField] private float roamingDistanceMax = 5f;
    [SerializeField] private float roamingDistanceMin = 3f;

    [SerializeField] private float roamingTimerMax = 5f;
    private float roamingTime = 10f;
    [SerializeField] private float idleTimerMax = 5f;
    private float idleTime = 5f;

    private NavMeshAgent navMeshAgent;
    private State commonState;
    private Vector3 roamingPosition;
    private Vector3 startingPosition;

    private enum State
    {
        Idle,
        Roaming,
        Attack,
        Dead
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        commonState = startingState;
    }

    private void Update()
    {
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
            transform.rotation = Quaternion.Euler(0, -180, 0);
        } else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
