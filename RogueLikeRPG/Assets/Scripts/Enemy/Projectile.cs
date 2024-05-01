using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    private Transform player;
    private Vector2 target;
    [SerializeField] private State projectileType;
    public int stat;

    private enum State{
        xp,
        arrow
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector2(player.position.x, player.position.y + 0.7f);
    }

    void Update()
    {
        if (projectileType == State.xp)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        } else if (projectileType == State.arrow)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        if (transform.position == new Vector3(target.x, target.y))
        {
            GetComponent<Collider2D>().enabled = false;
            enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && projectileType == State.xp)
        {
            LevelManager.Instance.IncreaseLevel(stat);
            Destroy(gameObject);
        } else if (other.CompareTag("Player") && projectileType == State.arrow)
        {
            other.GetComponent<Player>().TakeDamage(stat);
            Destroy(gameObject);
        }
    }
}
