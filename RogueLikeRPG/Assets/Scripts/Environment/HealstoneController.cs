using UnityEngine;

public class HealstoneController : MonoBehaviour
{
    public bool isUsed = false;
    public Animator animator;
    private int healnumber = 20;

    public void UseHealstone()
    {
        if (!isUsed)
        {
            isUsed = true;
            Player.Instance.Heal(healnumber);
            animator.SetBool("isUsed", isUsed);
        }
    }
}
