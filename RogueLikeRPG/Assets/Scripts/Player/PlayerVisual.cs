using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;
    private const string IS_WALKING = "IsWalking";
    private const string IS_DEAD = "IsDead";

    public Transform attackPoint;

    private bool _isHurt = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(IS_WALKING, Player.Instance.isWalking);
        AdjustPlayerFacingDirection();
    }

    public void SetAnimation(string animation)
    {
        animator.Play(animation);
    }

    public void SetDieAnimation()
    {
        animator.SetBool(IS_DEAD, true);
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

    public bool IsHurt()
    {
        return _isHurt;
    }

    public void HurtOff()
    {
        _isHurt = false;
    }

    public void HurtOn()
    {
        _isHurt = true;
    }
}
