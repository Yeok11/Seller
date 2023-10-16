using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    DataManager dataManager;
    internal float TimeData;
    OpenSystem OpSys;

    private void Awake()
    {
        dataManager = GameObject.Find("GameManager").GetComponent<DataManager>();
        OpSys = GameObject.Find("OpenSign").GetComponent<OpenSystem>();

        dataManager.Weeks.Add("일");
        dataManager.Weeks.Add("월");
        dataManager.Weeks.Add("화");
        dataManager.Weeks.Add("수");
        dataManager.Weeks.Add("목");
        dataManager.Weeks.Add("금");
        dataManager.Weeks.Add("토");
    }

    private void Start()
    {
        NewDay();
    }

    void NewDay()
    {
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = dataManager.Weeks[dataManager.Days % 7] + "요일";
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (dataManager.Days + 1) + "일";
    }

    private void HourTimer()
    {
        transform.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount = 1 - TimeData / 24;

        if (transform.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount == 0) OpSys.ChangeSign(2);
    }

    private void Update()
    {
        if(dataManager.NowOpen == true)
        {
            HourTimer();
            TimeData += Time.deltaTime;
        }
    }


}
