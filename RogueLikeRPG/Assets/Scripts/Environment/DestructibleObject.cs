using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    private bool isShacking = false;
    private float shake = .06f;
    private Vector2 pos;

    private int health = 2;

    private void Start()
    {
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
            Destroy(gameObject);
        }
        Invoke("StopShacking", .2f);
    }

    private void StopShacking()
    {
        isShacking = false;
        transform.position = pos;
    }
}
