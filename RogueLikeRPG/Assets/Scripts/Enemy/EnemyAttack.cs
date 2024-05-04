using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
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
        Instantiate(arrow, new Vector3(transform.position.x, transform.position.y + 0.7f), Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
