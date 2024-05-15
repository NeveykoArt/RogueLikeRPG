using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemySpawnPoints;
    public GameObject[] enemyPrefabs;

    private List<GameObject> enemies = new List<GameObject>();

    private bool visited = false;

    private int theMostRemotedRoom;
    private int currentRemoteness;
    private bool firstRoomFlag = false;

    public void FillData(int _theMostRemotedRoom, int _currentRemoteness, bool _firstRoomFlag)
    {
        theMostRemotedRoom = _theMostRemotedRoom;
        currentRemoteness = _currentRemoteness;
        firstRoomFlag = _firstRoomFlag;
    }

    public void SpawnEnemies()
    {
        if (!firstRoomFlag && !visited)
        {
            visited = true;
            if (currentRemoteness < theMostRemotedRoom / 2)
            {
                int lastPosition = -1;
                for (int i = 0; i < 3; i++)
                {
                    //комнаты со слизнями
                    if (lastPosition != 4)
                    {
                        while (true)
                        {
                            int newPosition = Random.Range(0, 5);
                            if (newPosition > lastPosition)
                            {
                                lastPosition = newPosition;
                                break;
                            }
                        }
                        enemies.Add(Instantiate(enemyPrefabs[Random.Range(3, 6)], 
                            enemySpawnPoints[lastPosition].transform.position, Quaternion.identity, transform));
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                int lastPosition = -1;
                for (int i = 0; i < 3; i++)
                {
                    //комнаты со скелетами
                    if (lastPosition != 4)
                    {
                        while (true)
                        {
                            int newPosition = Random.Range(0, 5);
                            if (newPosition > lastPosition)
                            {
                                lastPosition = newPosition;
                                break;
                            }
                        }
                        enemies.Add(Instantiate(enemyPrefabs[Random.Range(0, 3)], 
                            enemySpawnPoints[lastPosition].transform.position, Quaternion.identity, transform));
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    public void DeleteEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GetComponent<EnemyAI>().enemyAttack.DeleteArrows();
            Destroy(enemies[i]);
        }
        enemies.Clear();
    }
}