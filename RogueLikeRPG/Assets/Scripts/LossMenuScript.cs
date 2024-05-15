using TMPro;
using UnityEngine;

public class LossMenuScript : MonoBehaviour
{
    public TextMeshProUGUI currentPoints;
    public TextMeshProUGUI bestPoints;

    public void PrintPoints()
    {
        currentPoints.text = PlayerStats.Instance.points.ToString();
        bestPoints.text = PlayerStats.Instance.points.ToString();
    }
}
