using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    private Transform player;
    private Vector3 target;
    [SerializeField] private State projectileType;
    public int stat;

    private Vector3 direction;

    private enum State{
        xp,
        arrow,
        fireball
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector3(player.position.x, player.position.y + 0.4f);

        direction = target - transform.position;
        float rotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation - 180);
    }

    void Update()
    {
        if (projectileType == State.xp)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        if (projectileType == State.arrow || projectileType == State.fireball)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        if (transform.position == new Vector3(target.x, target.y))
        {
            if (projectileType == State.fireball)
            {
                GetComponent<Animator>().Play("BlastAnimation");
                GetComponent<Collider2D>().enabled = false;
                enabled = false;
            }
            else
            {
                GetComponent<Collider2D>().enabled = false;
                enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Walls"))
        {
            if (projectileType == State.fireball)
            {
                GetComponent<Animator>().Play("BlastAnimation");
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        if (other.CompareTag("Player"))
        {
            if (projectileType == State.xp)
            {
                LevelManager.Instance.IncreaseLevel(stat);
                Destroy(gameObject);
            }
            if (projectileType == State.fireball)
            {
                GetComponent<Animator>().Play("BlastAnimation");
                other.GetComponent<Player>().TakeDamage(stat);
            }
            if (projectileType == State.arrow)
            {
                other.GetComponent<Player>().TakeDamage(stat);
                Destroy(gameObject);
            }
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}

