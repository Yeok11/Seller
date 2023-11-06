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

    [SerializeField] internal GameObject WeekList;

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

    internal void NewDay()
    {
        DM.Days += 1;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DM.Weeks[DM.Days % 7] + "요일";
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = DM.Days + "일";

        SubSystemManager.Instance.MarketScheduleUpdate();
        StopCoroutine(CustomerManager.Instance.SpawnDelay());
        

        TimeData = 0;

        if (DM.Weeks[DM.Days % 7] == "수")
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
                OpSys.OnPointerDown(null);
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
