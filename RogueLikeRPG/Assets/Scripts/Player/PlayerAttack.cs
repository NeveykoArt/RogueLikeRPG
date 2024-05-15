using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 2f;
    public LayerMask enemyLayer;
    public LayerMask destructibleObjectLayer;

    public void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Boss"))
            {
                enemy.GetComponent<BossAI>().BossTakeDamage(PlayerStats.Instance.currentDamage);
            } else
            {
                enemy.GetComponent<EnemyAI>().EnemyTakeDamage(PlayerStats.Instance.currentDamage);
            }
        }
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, destructibleObjectLayer);
        foreach (Collider2D destrObj in hitObjects)
        {
            destrObj.GetComponent<DestructableObject>().TakeDamage();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
