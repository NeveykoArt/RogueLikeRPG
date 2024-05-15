using UnityEngine;

public class BossManager : MonoBehaviour
{
    public GameObject bossSpawnPoint;
    public GameObject bossPrefabs;

    private bool visited = false;

    public void SpawnBoss()
    {
        if (!visited)
        {
            visited = true;
            Instantiate(bossPrefabs, bossSpawnPoint.transform.position, Quaternion.identity, transform);
        }
    }
}
