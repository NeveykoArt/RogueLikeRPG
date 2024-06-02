using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public int bestTime = 0;
    public int bestPoints = 0;
}

public class SaveProgress : MonoBehaviour
{
    public PlayerInfo PlayerInfo;
    [DllImport("__Internal")]
    private static extern void SaveExtern(string date);
    [DllImport("__Internal")]
    private static extern void LoadExtern();
    public static SaveProgress Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            PlayerInfo = new PlayerInfo();
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            Instance = this;
            LoadExtern();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        string jsonString = JsonUtility.ToJson(PlayerInfo);
#if UNITY_WEBGL
        SaveExtern(jsonString);
#endif
    }

    public void SetPlayerInfo(string value)
    {
        PlayerInfo = JsonUtility.FromJson<PlayerInfo>(value);
    }
}
