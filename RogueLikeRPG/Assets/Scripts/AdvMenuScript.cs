using System.Collections;
using TMPro;
using UnityEngine;

public class AdvMenuScript : MonoBehaviour
{
    public GameObject advpanel;

    public TextMeshProUGUI timer;

    public static AdvMenuScript Instance {  get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void ShowAdv()
    {
        advpanel.SetActive(true);
        StartCoroutine(Adv());
    }

    private IEnumerator Adv()
    {
        int k = 3;
        while (k != 0)
        {
            timer.text = k.ToString();
            k--;
            yield return new WaitForSeconds(1);
        }
        Yandex.Instance.ShowAdvertisement();
        yield return new WaitForSeconds(2);
        advpanel.SetActive(false);

    }
}
