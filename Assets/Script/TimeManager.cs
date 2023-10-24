using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimeManager : SingleTon<TimeManager>
{
    DataManager DM;
    internal float TimeData;
    OpenSystem OpSys;

    private void Awake()
    {
        DM = DataManager.Instance;
        OpSys = GameObject.Find("OpenSign").GetComponent<OpenSystem>();

        DM.Weeks.Add("��");
        DM.Weeks.Add("��");
        DM.Weeks.Add("ȭ");
        DM.Weeks.Add("��");
        DM.Weeks.Add("��");
        DM.Weeks.Add("��");
        DM.Weeks.Add("��");
    }

    private void Start()
    {
        NewDay();
    }

    internal void NewDay()
    {
        DM.Days += 1;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DM.Weeks[DM.Days % 7] + "����";
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = DM.Days + "��";

        SubSystemManager.Instance.MarketScheduleUpdate();

        TimeData = 0;

        if (DM.Weeks[DM.Days % 7] == "��")
        {
            CounterManager.Instance.AddItem();
        }

        HourTimer();
    }

    internal void HourTimer()
    {
        transform.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount = 1 - TimeData / 24;

        if (transform.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount == 0 && DM.NowOpen == true) OpSys.OnPointerDown(null);
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
