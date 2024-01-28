using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    private Animator EnemyAnimator;

    private const string IS_ROAMING = "IsRoaming";
    private const string IS_ATTACKING = "IsAttacking";
    private const string IS_DEAD = "IsDead";

    private void Awake()
    {
        EnemyAnimator = GetComponent<Animator>();
    }
    public void SetRoamingAnimation(bool isRoaming)
    {
        EnemyAnimator.SetBool(IS_ROAMING, isRoaming);
    }
    public void SetAttackingAnimation(bool isAttacking)
    {
        EnemyAnimator.SetBool(IS_ATTACKING, isAttacking);
    }
    public void SetDeadAnimation(bool isDead)
    {
        EnemyAnimator.SetBool(IS_DEAD, isDead);
    }
}
