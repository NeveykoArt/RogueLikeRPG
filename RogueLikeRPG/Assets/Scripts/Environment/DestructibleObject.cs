using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    private bool isShacking = false;
    private float shake = .06f;
    private Vector2 pos;

    private int health = 2;

    public GameObject destroyed;
    private GameObject destr;

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
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        destr = Instantiate(destroyed, transform.position, Quaternion.identity);
    }

    public void DeleteDestroyed()
    {
        Destroy(destr);
    }
}
