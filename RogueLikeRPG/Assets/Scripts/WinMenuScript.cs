using TMPro;
using UnityEngine;

public class WinMenuScript : MonoBehaviour
{
    public TextMeshProUGUI currentTime;
    public TextMeshProUGUI bestTime;

    public GameObject rateButton;

    public void PrintTime()
    {
        var hour = TextManager.Instance.hour;
        var min = TextManager.Instance.min;
        var sec = TextManager.Instance.sec;

        currentTime.text = hour.ToString("D2") + " : " + min.ToString("D2") + " : " + sec.ToString("D2");

        if ((hour * 3600 + min * 60 + sec < SaveProgress.Instance.PlayerInfo.bestTime)
            || (SaveProgress.Instance.PlayerInfo.bestTime == 0))
        {
            SaveProgress.Instance.PlayerInfo.bestTime = hour * 3600 + min * 60 + sec;
            Yandex.Instance.SetTimeToLeaderboard(SaveProgress.Instance.PlayerInfo.bestTime * 1000);
            if (Yandex.Instance.status)
            {
                rateButton.SetActive(true);
            }
        }

        int bestHour = SaveProgress.Instance.PlayerInfo.bestTime / 3600;
        int bestMin = (SaveProgress.Instance.PlayerInfo.bestTime % 3600) / 60;
        int bestSec = (SaveProgress.Instance.PlayerInfo.bestTime % 3600) % 60;

        bestTime.text = bestHour.ToString("D2") + " : " + bestMin.ToString("D2") + " : " + bestSec.ToString("D2");
        SaveProgress.Instance.Save();
    }
}
