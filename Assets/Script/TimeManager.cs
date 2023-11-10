using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimeManager : SingleTon<TimeManager>
{
    DataManager DM;
    [SerializeField] OpenSystem OPS;
    internal float TimeData;

    [SerializeField] internal GameObject WeekList;

    private void Awake()
    {
        DM = DataManager.Instance;

        DM.Weeks = new List<string>(new string[]
        {
            "��",
            "��",
            "ȭ",
            "��",
            "��",
            "��",
            "��"
        });
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
        StopCoroutine(CustomerManager.Instance.SpawnDelay());
        

        TimeData = 0;

        if (DM.Weeks[DM.Days % 7] == "��")
        {
            CounterManager.Instance.AddItem();
        }
        else if (DM.Days % 7 ==0)
        {
            OpenSystem.Check_WeekList = false;
        }
        

        HourTimer();
    }

    internal void HourTimer()
    {
        transform.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount = 1 - TimeData / (24 + (24 * DM.BonusPer[0, DM.InteriorLevel[0] - 1] / 100));

        if (transform.GetChild(2).GetChild(0).GetComponent<Image>().fillAmount == 0)
        {
            CustomerManager.Instance.OrderNowDo = false;
            if (DM.NowOpen == true)
            {
                OPS.OnPointerDown(null);
            }
        } 
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
