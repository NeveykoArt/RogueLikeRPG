using System.Collections.Generic;
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

    public GameObject PlayerPosition;

    private GameObject portal;
    private List<GameObject> blocks = new List<GameObject>();
    private GameObject healstone;

    public void UpdateRoom(DungeonGenerator.Cell currentCell)
    {
        for (int i = 0; i < currentCell.status.Length; i++)
        {
            doors[i].SetActive(currentCell.status[i]);
            walls[i].SetActive(!currentCell.status[i]);
            if (currentCell.lastRoom && currentCell.status[i])
            {
                portal = Instantiate(portalObject, portals[i].transform.position, Quaternion.identity);
                portal.name += "_" + currentCell.index;
            }
        }
        if (currentCell.healstonePosition != 4)
        {
            healstone = Instantiate(healstoneObject, healstones[currentCell.healstonePosition].transform.position, Quaternion.identity);
            healstone.name += "_" + currentCell.index;
        }
        for (int i = 0; i < currentCell.blockagePosition.Count; i++)
        {
            var block = Instantiate(blockageObject, blockages[currentCell.blockagePosition[i]].transform.position, Quaternion.identity);
            block.name += "_" + currentCell.index;
            blocks.Add(block);
        }
        if (currentCell.firstRoom)
        {
            Player.Instance.GetComponent<Transform>().position = PlayerPosition.transform.position;
            Camera.main.transform.position = new Vector3(PlayerPosition.transform.position.x, PlayerPosition.transform.position.y, -10);
        }
    }

    public void DeleteProceduralObjects()
    {
        for(int i = 0; i < blocks.Count; i++)
        {
            blocks[i].GetComponent<DestructableObject>().DeleteDestroyed();
            Debug.Log($"{blocks[i].name} was deleted");
            Destroy(blocks[i]);
        }
        if (healstone != null)
        {
            Debug.Log($"{healstone.name} was deleted");
            Destroy(healstone);
        }
        if (portal != null)
        {
            Debug.Log($"{portal.name} was deleted");
            portal.GetComponent<PortalController>().DeleteInteractElement();
            Destroy(portal);
        }
    }
}
