using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private List<GameObject> arrows = new List<GameObject>();

    public int damage = 10;
    public Transform attackPoint;
    public float attackRange = 2f;
    public LayerMask playerLayer;
    public GameObject arrow;

    public void Attack()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        if (hitPlayer != null)
        {
            hitPlayer.GetComponent<Player>().TakeDamage(damage);
        }
    }

    public void Shoot()
    {
        arrows.Add(Instantiate(arrow, new Vector3(transform.position.x, transform.position.y + 0.7f), Quaternion.identity));
    }

    public void DeleteArrows()
    {
        for(int i = 0; i < arrows.Count; i++)
        {
            Destroy(arrows[i]);
        }
        arrows.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
