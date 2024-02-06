using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private const string IS_WALKING = "IsWalking";
    private const string IS_DEAD = "IsDead";
    private const string ATTACK = "Attack";

    public Transform attackPoint;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        animator.SetBool(IS_WALKING, Player.Instance.IsWalking());
        AdjustPlayerFacingDirection();
    }

    public void SetCombatAnimation()
    {
        animator.SetTrigger(ATTACK);
    }

    public void SetDieAnimation()
    {
        animator.SetBool(IS_DEAD, true);
    }

    public void SetHurtAnimation()
    {
        animator.Play("Hurt");
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 playerPos = Player.Instance.GetPlayerScreenPosition();
        if (mousePos.x > playerPos.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
    }
}
