using System.Runtime.InteropServices;
using UnityEngine;

public class Yandex : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void AuthStatus();

    [DllImport("__Internal")]
    private static extern void RateGame();

    [DllImport("__Internal")]
    private static extern void ShowAdv();

    [DllImport("__Internal")]
    private static extern void SetToPointsLeaderboard(int points);

    [DllImport("__Internal")]
    private static extern void SetToTimeLeaderboard(int time);

    public bool status = false;

    public static Yandex Instance;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        GetAuthentificationStatus();
    }

    public void GetAuthentificationStatus()
    {
        AuthStatus();
    }

    public void AuthentificationStatus()
    {
        status = true;
    }

    public void OffAuthentificationStatus()
    {
        status = false;
    }

    public void RateGameButton()
    {
        RateGame();
    }

    public void ShowAdvertisement()
    {
        ShowAdv();
    }

    public void SetPointsToLeaderboard(int points)
    {
        SetToPointsLeaderboard(points);
    }

    public void SetTimeToLeaderboard(int time)
    {
        SetToTimeLeaderboard(time);
    }
}