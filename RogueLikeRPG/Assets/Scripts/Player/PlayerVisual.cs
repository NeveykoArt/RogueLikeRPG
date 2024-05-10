using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Animator animator;
    private const string IS_WALKING = "IsWalking";
    private const string IS_DEAD = "IsDead";
    private const string IS_PROTECT = "IsProtect";

    public Transform attackPoint;

    public bool _isHurt = false;
    public bool dashFlag = true;
    private float dashDelay = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (dashDelay <= Time.time)
        {
            dashDelay = Time.time + 3f;
            dashFlag = true;
        }

        animator.SetBool(IS_WALKING, Player.Instance.isWalking);
        animator.SetBool(IS_PROTECT, Player.Instance.isProtect);
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

    public bool isHurt()
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

    public void SetDash()
    {
        dashFlag = false;
    }
}
