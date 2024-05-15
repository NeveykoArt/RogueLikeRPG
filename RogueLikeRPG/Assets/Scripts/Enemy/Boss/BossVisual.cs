using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BossVisual : MonoBehaviour
{
    private Animator BossAnimator;

    private const string IS_WALKING = "IsWalking";
    private const string IS_DEAD = "IsDead";

    private void Awake()
    {
        BossAnimator = GetComponent<Animator>();
    }

    public void SetWalkingAnimation(bool isWalking)
    {
        BossAnimator.SetBool(IS_WALKING, isWalking);
    }

    public void SetChasingAnimation()
    {
        BossAnimator.Play("BossChase");
    }

    public void SetAnimation(string animation)
    {
        BossAnimator.Play(animation);
    }

    public void StopAnimation()
    {
        TextManager.Instance.StopTimer();
        BossHealthBarScript.Instance.SetWinMenu();
        enabled = false;
    }

    public void SetDeadAnimation()
    {
        GetComponent<ShadowCaster2D>().enabled = false;
        BossAnimator.SetBool(IS_DEAD, true);
    }

    public void ChangeFasing(Quaternion rotation)
    {
        transform.rotation = rotation;
    }
}
