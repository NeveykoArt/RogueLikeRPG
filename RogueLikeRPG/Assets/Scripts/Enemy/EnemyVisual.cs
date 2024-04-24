using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyVisual : MonoBehaviour
{
    private Animator EnemyAnimator;

    private const string IS_ROAMING = "IsRoaming";
    private const string IS_RUNNING = "IsRunning";
    private const string IS_DEAD = "IsDead";

    public bool _hurtFlag = false;

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

    public void StopAnimation()
    {
        enabled = false;
    }

    public void SetRunningAnimation(bool isRunning)
    {
        EnemyAnimator.SetBool(IS_RUNNING, isRunning);
    }

    public void SetDeadAnimation()
    {
        GetComponent<ShadowCaster2D>().enabled = false;
        EnemyAnimator.SetBool(IS_DEAD, true);
    }

    public void ChangeFasing(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    public bool IsHurt()
    {
        return _hurtFlag;
    }

    public void HurtOff()
    {
        _hurtFlag = false;
    }

    public void HurtOn()
    {
        _hurtFlag = true;
    }
}
