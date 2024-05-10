using UnityEngine;

public class SpawnBossScript : MonoBehaviour
{
    public GameObject room;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out _))
        {
            room.GetComponent<BossManager>().SpawnBoss();
        }
    }
}
