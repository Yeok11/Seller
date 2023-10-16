using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    DataManager DM;
    internal float TimeData;
    OpenSystem OpSys;

    private void Awake()
    {
        DM = DataManager.Instance;
        OpSys = GameObject.Find("OpenSign").GetComponent<OpenSystem>();

        DM.Weeks.Add("일");
        DM.Weeks.Add("월");
        DM.Weeks.Add("화");
        DM.Weeks.Add("수");
        DM.Weeks.Add("목");
        DM.Weeks.Add("금");
        DM.Weeks.Add("토");
    }

    private void Start()
    {
        NewDay();
    }

    private void NewDay()
    {
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DM.Weeks[DM.Days % 7] + "요일";
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (DM.Days + 1) + "일";
    }

    private void HourTimer()
    {
        transform.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount = 1 - TimeData / 24;

        if (transform.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount == 0) OpSys.OnPointerDown(null);
    }

    private void Update()
    {
        if(DM.NowOpen == true)
        {
            HourTimer();
            TimeData += Time.deltaTime;
        }
    }


}
