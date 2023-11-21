using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TimeManager : SingleTon<TimeManager>
{
    DataManager DM;
    [SerializeField] OpenSystem OPS;
    internal float TimeData;

    [SerializeField] internal GameObject WeekList;

    [SerializeField] private Image DayUpdateScene;
    private float UpdateSceneCnt;
    [SerializeField] GameObject End;

    [SerializeField] AudioSource Bgm;


    private List<string> DayEvent = new List<string>();

    private void Awake()
    {
        DM = DataManager.Instance;

        for (int i = 0; i < CSVManager.Instance.csvdata.DayEvent.Count; i++) DayEvent.Add(CSVManager.Instance.csvdata.DayEvent[i]["Mes"].ToString());

        DM.Weeks = new List<string>(new string[]
        {
            "ÀÏ", "¿ù", "È­", "¼ö", "¸ñ", "±Ý", "Åä"
        });
    }

    private void Start()
    {
        NewDay();
    }

    IEnumerator NewDayMes()
    {
        DayUpdateScene.color = new Color(0, 0, 0, 1);
        DayUpdateScene.transform.GetChild(0).gameObject.SetActive(true);
        DayUpdateScene.GetComponentInChildren<TextMeshProUGUI>().text = $"Day - {DM.Days}";

        newDayEvent();

        yield return new WaitForSeconds(2f);

        while (true)
        {
            yield return new WaitForSeconds(0.005f);

            UpdateSceneCnt -= Time.time;

            DayUpdateScene.color = new Color(0, 0, 0, UpdateSceneCnt);

            if (UpdateSceneCnt <= 0.7) DayUpdateScene.transform.GetChild(0).gameObject.SetActive(false);

            if (UpdateSceneCnt <= 0)
            {
                DayUpdateScene.gameObject.SetActive(false);
                break;
            }
        }
        yield return null;
    }

    private void newDayEvent()
    {
        int EventNum = UnityEngine.Random.Range(0, CSVManager.Instance.csvdata.DayEvent.Count);

        DayUpdateScene.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = DayEvent[EventNum];
    }

    internal void NewDay()
    {
        DM.Days += 1;

        UpdateSceneCnt = 1;
        DayUpdateScene.gameObject.SetActive(true);
        StartCoroutine("NewDayMes");

        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DM.Weeks[DM.Days % 7] + "¿äÀÏ";
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = DM.Days + "ÀÏ";

        SubSystemManager.Instance.MarketScheduleUpdate();
        StopCoroutine("SpawnDelay");
        
        TimeData = 0;

        if (DM.Days == DM.MaxDay)
        {
            End.SetActive(true);
            Bgm.Stop();
            End.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "ÃÑ ÁöÃâ¾× : " + DM.UseGold[0];
            End.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "ÃÑ ¸ÅÃâ¾× : " + DM.BuyGold[0];
            End.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = "ÃÑ Â÷¾× : " + (DM.BuyGold[0] - DM.UseGold[0]);
            End.transform.GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>().text = "ÃÑ ¼Õ´Ô ¹æ¹® È½¼ö : " + DM.ComeCustomerCnt[0];
            End.transform.GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>().text = "½Ç¼ö È½¼ö : " + DM.MissCnt[0];
            End.transform.GetChild(1).GetChild(5).GetComponent<TextMeshProUGUI>().text = "ÃÑ ÆÇ¸Å È½¼ö : " + DM.SellCnt[0];
            End.transform.GetChild(1).GetChild(6).GetComponent<TextMeshProUGUI>().text = "ÃÖÁ¾ º¸À¯ ±Ý¾× : " + DM.HaveMoney;

            if (PlayerPrefs.GetInt("HaveMoney") < DM.HaveMoney) PlayerPrefs.SetInt("HaveMoney", DM.HaveMoney);
            PlayerPrefs.SetInt("Customer", PlayerPrefs.GetInt("Customer") + DM.ComeCustomerCnt[0]);
            PlayerPrefs.SetInt("AllMiss", PlayerPrefs.GetInt("AllMiss") + DM.MissCnt[0]);
            if (PlayerPrefs.GetInt("Miss") < DM.MissCnt[0]) PlayerPrefs.SetInt("Miss", DM.MissCnt[0]);
            PlayerPrefs.SetInt("AllBuy", PlayerPrefs.GetInt("AllBuy") + DM.BuyCnt);
            if (PlayerPrefs.GetInt("Buy") < DM.BuyCnt) PlayerPrefs.SetInt("Buy", DM.BuyCnt);
            PlayerPrefs.SetInt("AllSell", PlayerPrefs.GetInt("AllSell") + DM.SellCnt[0]);
            if (PlayerPrefs.GetInt("Sell") < DM.SellCnt[0]) PlayerPrefs.SetInt("Sell", DM.SellCnt[0]);
            if (Title.BestGetMoney[(int)DataManager.GameDif] < DM.BuyGold[0]) Title.BestGetMoney[(int)DataManager.GameDif] = DM.BuyGold[0];



            return;
        }

        if (DM.Weeks[DM.Days % 7] == "¼ö") CounterManager.Instance.AddItem();
        else if (DM.Days % 7 ==0) OpenSystem.Check_WeekList = false;


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
