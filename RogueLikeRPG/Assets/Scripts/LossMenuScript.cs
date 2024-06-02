using System.Collections;
using TMPro;
using UnityEngine;

public class LossMenuScript : MonoBehaviour
{
    public TextMeshProUGUI currentPoints;
    public TextMeshProUGUI bestPoints;

    public GameObject rateButton;

    public void PrintPoints()
    {
        currentPoints.text = PlayerStats.Instance.points.ToString();

        if (PlayerStats.Instance.points > SaveProgress.Instance.PlayerInfo.bestPoints)
        {
            SaveProgress.Instance.PlayerInfo.bestPoints = PlayerStats.Instance.points;
            Yandex.Instance.SetPointsToLeaderboard(PlayerStats.Instance.points);
            if (Yandex.Instance.status)
            {
                rateButton.SetActive(true);
            }
        }

        bestPoints.text = SaveProgress.Instance.PlayerInfo.bestPoints.ToString();
        SaveProgress.Instance.Save();
    }
}
