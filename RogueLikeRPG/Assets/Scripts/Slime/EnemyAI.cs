using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Utils;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State startingState;
    [SerializeField] private float roamingDistanceMax = 7f;
    [SerializeField] private float roamingDistanceMin = 3f;
    [SerializeField] private float roamingTimerMax = 2f;

    private NavMeshAgent navMeshAgent;
    private State commonState;
    private float roamingTime;
    private Vector3 roamingPosition;
    private Vector3 startingPosition;

    private enum State
    {
        Idle,
        Roaming
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
        startingPosition = transform.position;
    }

    private void Update()
    {
        switch (commonState)
        {
            default:
            case State.Idle:
                break;
            case State.Roaming:
                roamingTime -= Time.deltaTime;
                if (roamingTime < 0)
                {
                    Roaming();
                    roamingTime = roamingTimerMax;
                }
                break;
        }
    }
    private void Roaming()
    {
        roamingPosition = GetRoamingPosition();
        navMeshAgent.SetDestination(roamingPosition);
    }
    private Vector3 GetRoamingPosition()
    {
        return startingPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(roamingDistanceMin, roamingDistanceMax);
    }
}
