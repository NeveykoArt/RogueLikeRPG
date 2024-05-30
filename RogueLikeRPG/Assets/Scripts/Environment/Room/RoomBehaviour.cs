using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] walls;
    public GameObject[] doors;
    public GameObject[] blockagePoints;
    public GameObject[] healstonePoints;
    public GameObject[] portalPoints;

    public GameObject blockagePrefab;
    public GameObject healstonePrefab;
    public GameObject portalPrefab;

    public GameObject PlayerPosition;

    public void UpdateRoom(DungeonGenerator.Cell currentCell, bool typeOfGame)
    {
        for (int i = 0; i < currentCell.status.Length; i++)
        {
            doors[i].SetActive(currentCell.status[i]);
            walls[i].SetActive(!currentCell.status[i]);
            if (currentCell.lastRoom && currentCell.status[i] && !typeOfGame)
            {
                var portal = Instantiate(portalPrefab, 
                    portalPoints[i].transform.position, Quaternion.identity, transform);
                portal.name += "_" + currentCell.index;
            }
        }

        if (currentCell.healstonePosition != 4)
        {
            var healstone = Instantiate(healstonePrefab, 
                healstonePoints[currentCell.healstonePosition].transform.position, Quaternion.identity, transform);
            healstone.name += "_" + currentCell.index;
        }

        for (int i = 0; i < currentCell.blockagePosition.Count; i++)
        {
            var block = Instantiate(blockagePrefab, 
                blockagePoints[currentCell.blockagePosition[i]].transform.position, Quaternion.identity, transform);
            block.name += "_" + currentCell.index;
        }

        if (currentCell.firstRoom)
        {
            Player.Instance.transform.position = PlayerPosition.transform.position;
            Camera.main.transform.position = new Vector3(PlayerPosition.transform.position.x, 
                PlayerPosition.transform.position.y, -10);
        }

        if (!(typeOfGame && currentCell.lastRoom))
        {
            gameObject.GetComponent<EnemyManager>().FillData(currentCell.theMostRemotedRoom, 
                currentCell.remoteness, currentCell.firstRoom);
        }
    }

    public void DeleteRoomInformation()
    {
        gameObject.GetComponent<EnemyManager>().DeleteEnemies();
        for (int i = 0; i < 4; i++) 
        {
            doors[i].SetActive(false);
            walls[i].SetActive(false);
        }
        AstarPath.active.Scan();
    }
}