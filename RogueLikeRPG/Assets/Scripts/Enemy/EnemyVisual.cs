using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    private Animator EnemyAnimator;

    private const string IS_ROAMING = "IsRoaming";
    private const string IS_RUNNING = "IsRunning";
    private const string IS_DEAD = "IsDead";

    private void Awake()
    {
        EnemyAnimator = GetComponent<Animator>();
    }
    public void SetRoamingAnimation(bool isRoaming)
    {
        EnemyAnimator.SetBool(IS_ROAMING, isRoaming);
    }
    public void SetAnimation(string animation)
    {
        EnemyAnimator.Play(animation);
    }
    public void SetRunningAnimation(bool isRunning)
    {
        EnemyAnimator.SetBool(IS_RUNNING, isRunning);
    }
    public void SetDeadAnimation()
    {
        EnemyAnimator.SetBool(IS_DEAD, true);
    }
    public void ChangeFasing(Quaternion rotation)
    {
        transform.rotation = rotation;
    }
}
