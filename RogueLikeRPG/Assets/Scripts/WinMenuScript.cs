using TMPro;
using UnityEngine;

public class WinMenuScript : MonoBehaviour
{
    public TextMeshProUGUI currentTime;
    public TextMeshProUGUI bestTime;

    void Update()
    {
        currentTime.text = TextManager.Instance.hour.ToString("D2") + 
            " : " + TextManager.Instance.min.ToString("D2") + " : " + TextManager.Instance.sec.ToString("D2");
        bestTime.text = TextManager.Instance.hour.ToString("D2") +
            " : " + TextManager.Instance.min.ToString("D2") + " : " + TextManager.Instance.sec.ToString("D2");
    }
}
