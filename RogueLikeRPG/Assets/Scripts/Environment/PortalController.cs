using UnityEngine;

public class PortalController : MonoBehaviour
{
    public void PortalActivation()
    {
        DungeonGenerator.Instance.RebuildDungeon();
        AdvMenuScript.Instance.ShowAdv();
    }
}
