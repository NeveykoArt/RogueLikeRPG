using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public int damage = 40;
    public Transform attackPoint;
    public float attackRange = 2f;
    public LayerMask playerLayer;
    public GameObject fireball;

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
        Instantiate(fireball, new Vector3(transform.position.x, transform.position.y + 1f), Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
