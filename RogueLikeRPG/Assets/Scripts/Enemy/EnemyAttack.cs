using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public EnemyVisual enemyVisual;

    private int damage = 10;
    public Transform attackPoint;
    public float attackRange = 2f;
    public LayerMask playerLayer;

    public void Attack()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        if (hitPlayer != null)
        {
            hitPlayer.GetComponent<Player>().TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
