using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] walls;
    public GameObject[] doors;
    public GameObject[] blockages;
    public GameObject[] healstones;
    public GameObject[] portals;

    public GameObject blockageObject;
    public GameObject healstoneObject;
    public GameObject portalObject;

    public void UpdateRoom(DungeonGenerator.Cell currentCell)
    {
        for (int i = 0; i < currentCell.status.Length; i++)
        {
            doors[i].SetActive(currentCell.status[i]);
            walls[i].SetActive(!currentCell.status[i]);
            if (currentCell.lastRoom && currentCell.status[i])
            {
                Instantiate(portalObject, portals[i].transform.position, Quaternion.identity);
            }
        }
        if (currentCell.healstonePosition != 4)
        {
            Instantiate(healstoneObject, healstones[currentCell.healstonePosition].transform.position, Quaternion.identity);
        }
        for (int i = 0; i < currentCell.blockagePosition.Count; i++)
        {
            Instantiate(blockageObject, blockages[currentCell.blockagePosition[i]].transform.position, Quaternion.identity);
        }
    }
}
