using UnityEngine;

public class PortalController : MonoBehaviour
{
    public Object interactObject;
    public void UsePortal()
    {
        DungeonGenerator.Instance.RebuildDungeon();
        Debug.Log("Portal to next level used");
    }
    public void DeleteInteractElement()
    {
        Destroy(interactObject);
    }
}
