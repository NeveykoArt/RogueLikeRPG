using System.Collections;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public static TextManager Instance { get; private set; }

    public bool typeOfGame = false;

    public TextMeshProUGUI textPoint;
    public TextMeshProUGUI textDungeonCount;
    public TextMeshProUGUI timer;

    public int sec = 0;
    public int min = 0;
    public int hour = 0;
    [SerializeField] private int delta = 0;

    public Coroutine timerCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (!typeOfGame)
        {
            textPoint.text = PlayerStats.Instance.points.ToString();
            textDungeonCount.text = PlayerStats.Instance.dungeons.ToString();
        } else
        {
            timerCoroutine = StartCoroutine(ITimer());
        }
    }

    void Update()
    {
        if (!typeOfGame)
        {
            textPoint.text = PlayerStats.Instance.points.ToString();
            textDungeonCount.text = PlayerStats.Instance.dungeons.ToString();
        }
    }

    IEnumerator ITimer()
    {
        while(true)
        {
            if (sec == 59)
            {
                min++;
                sec = -1;
            }
            if (min == 60)
            {
                hour++;
                min = 0;
            }
            sec += delta;
            timer.text = hour.ToString("D2") + " : " + min.ToString("D2") + " : " + sec.ToString("D2");
            yield return new WaitForSeconds(1);
        }
    }

    public void StopTimer()
    {
        StopCoroutine(timerCoroutine);
    }
}