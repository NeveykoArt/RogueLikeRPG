using TMPro;
using UnityEngine;

public class LossMenuScript : MonoBehaviour
{
    public TextMeshProUGUI currentPoints;
    public TextMeshProUGUI bestPoints;

    public void PrintPoints()
    {
        currentPoints.text = PlayerStats.Instance.points.ToString();

        if (PlayerStats.Instance.points > SaveProgress.Instance.PlayerInfo.bestPoints)
        {
            SaveProgress.Instance.PlayerInfo.bestPoints = PlayerStats.Instance.points;
        }

        bestPoints.text = SaveProgress.Instance.PlayerInfo.bestPoints.ToString();
        SaveProgress.Instance.Save();
    }
}
