using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    private bool isShacking = false;
    private float shake = .06f;
    private Vector2 pos;

    private int health = 2;

    public Object destroyed;

    private void Start()
    {
        pos = transform.position;
    }

    private void Update()
    {
        if (isShacking == true)
        {
            transform.position = pos + UnityEngine.Random.insideUnitCircle * shake;
        }
    }

    public void TakeDamage()
    {
        isShacking = true;
        health--;
        if (health <= 0)
        {
            DestroyTheObject();
        }
        Invoke("StopShacking", .2f);
    }

    private void StopShacking()
    {
        isShacking = false;
        transform.position = pos;
    }

    private void DestroyTheObject()
    {
        Instantiate(destroyed, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
