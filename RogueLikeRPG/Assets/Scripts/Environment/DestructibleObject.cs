using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    private Animator animator;

    private bool isShacking = false;
    private float shake = .06f;
    private Vector2 pos;

    private int health = 2;

    private void Start()
    {
        animator = GetComponent<Animator>();
        pos = transform.position;
    }

    private void Update()
    {
        if (isShacking == true)
        {
            transform.position = pos + Random.insideUnitCircle * shake;
        }
    }

    public void TakeDamage()
    {
        isShacking = true;
        health--;
        if (health <= 0)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            animator.SetBool("isExplosion", true);
        }
        Invoke("StopShacking", .2f);
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void StopShacking()
    {
        isShacking = false;
        transform.position = pos;
    }
}
