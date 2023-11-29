using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
//using System;

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

    private List<string> _DayEvent = new List<string>();

    internal int[] WeekBonusValue = new int[] { 0, 0, 0 }; //판매가 , 매입가
    internal bool[] WeekEventBoolTrigger = { false, false, false }; //특수 이벤트 여부, 관광객 발길, 소리
    internal int[] EventDayContinue = new int[10];
    internal int DayMesCode;

    private void Awake()
    {
        DM = DataManager.Instance;

        for (int i = 0; i < CSVManager.Instance.csvdata.DayEvent.Count; i++) _DayEvent.Add(CSVManager.Instance.csvdata.DayEvent[i]["Mes"].ToString());

        DM.Weeks = new List<string>(new string[] { "일", "월", "화", "수", "목", "금", "토" });
    }


    IEnumerator NewDayMes()
    {
        DayUpdateScene.color = new Color(0, 0, 0, 1);
        DayUpdateScene.transform.GetChild(0).gameObject.SetActive(true);
        DayUpdateScene.GetComponentInChildren<TextMeshProUGUI>().text = $"Day - {DM.Days}";

        if (DataManager.GameDif == Diff.Event_1)
        {
            DayUpdateScene.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("왜인지 심상치 않은 장사가 될 것 같습니다.");
        }
        else
        {
            if (TitleManager.ContinueData)
            {
                if (WeekEventBoolTrigger[0]) DayUpdateScene.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().SetText(CSVManager.Instance.csvdata.DayEvent[DayMesCode]["WeekEvent"].ToString());
                else
                {
                    DayUpdateScene.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().SetText(_DayEvent[DayMesCode]);
                }

                TitleManager.ContinueData = false;
            }
            else
            {
                if (UnityEngine.Random.Range(0, 100) < 10)
                {
                    try
                    {
                        if (DM.Weeks[DM.Days % 7] == "월") DayUpdateScene.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().SetText(CSVManager.Instance.csvdata.DayEvent[_newDayEvent(true)]["WeekEvent"].ToString());
                        else DayUpdateScene.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().SetText(_DayEvent[_newDayEvent(false)]);
                    }
                    catch
                    {
                        DayUpdateScene.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().SetText(_DayEvent[_newDayEvent(false)]);
                    }
                }
                else DayUpdateScene.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().SetText(_DayEvent[_newDayEvent(false)]);
            }
        }
        
        

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

    private int _newDayEvent(bool Mon)
    {
        if (Mon)
        {
            DayMesCode = 999;
            for (int i = 0; i < CSVManager.Instance.csvdata.DayEvent.Count; i++)
            {
                if (CSVManager.Instance.csvdata.DayEvent[i]["WeekEvent"].ToString() == "")
                {
                    DayMesCode = UnityEngine.Random.Range(0, i);
                    break;
                }
            }

            /*주간 이벤트
             * 판매가: WeekBonusValue[0] += Value;
             * 매입가: WeekBonusValue[1] = Value;
             * Bool이벤트
            */
            switch (DayMesCode)
            {
                case 0:
                    WeekBonusValue[0] += 10;
                    EventDayContinue[DayMesCode] = 5;
                    break;

                case 1:
                    WeekBonusValue[0] += 20;
                    EventDayContinue[DayMesCode] = 5;
                    break;

                case 2:
                    WeekBonusValue[0] += 35;
                    EventDayContinue[DayMesCode] = 5;
                    break;

                case 3:
                    WeekBonusValue[1] += 10;
                    EventDayContinue[DayMesCode] = 5;
                    break;

                case 4:
                    WeekBonusValue[1] += 15;
                    EventDayContinue[DayMesCode] = 5;
                    break;

                case 5:
                    if(!WeekEventBoolTrigger[0])DM.HaveMoney += 50000;
                    break;

                case 6:
                    WeekEventBoolTrigger[1] = true;
                    EventDayContinue[DayMesCode] = 5;
                    break;

                case 7:
                    WeekEventBoolTrigger[2] = true;
                    EventDayContinue[DayMesCode] = 5;
                    break;
            }
            WeekEventBoolTrigger[0] = true;
            return DayMesCode;
        }
        else
        {
            if (DM.Weeks[DM.Days % 7] == "수") return DayMesCode = CSVManager.Instance.csvdata.DayEvent.Count - 1;
        }
        DayMesCode = UnityEngine.Random.Range(0, CSVManager.Instance.csvdata.DayEvent.Count - 1);
        return DayMesCode;
    }

    internal void NewDay()
    {
        DM.Days += 1;

        UpdateSceneCnt = 1;
        DayUpdateScene.gameObject.SetActive(true);
        StartCoroutine("NewDayMes");

        SubSystemManager.Instance.CanUseBt = true;

        if(DataManager.GameDif != Diff.Event_1)
        {
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DM.Weeks[DM.Days % 7] + "요일";
            transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = DM.Days + "일";

            SubSystemManager.Instance.MarketScheduleUpdate();
            if (DM.Weeks[DM.Days % 7] == "수") CounterManager.Instance.AddItem();
            else if (DM.Days % 7 == 0) OpenSystem.Check_WeekList = false;
        }
        StopCoroutine("SpawnDelay");

        if (DataManager.GameDif == Diff.Event_1)
        {
            WeekList.SetActive(true);

            for (int i = 0; i < DataManager.Instance.ItemCount.Length; i++) DataManager.Instance.ItemCount[i] = 99;
        }
        CounterManager.Instance.ShelfReset();

        TimeData = 0;

        if (DM.Days == DM.MaxDay)
        {
            Ending();
            return;
        }

        if (WeekEventBoolTrigger[0])
        {
            for (int i = 0; i < EventDayContinue.Length; i++)
            {
                if (EventDayContinue[i] != 0)
                {
                    EventDayContinue[i]--;
                    if (EventDayContinue[i] == 0)
                    {
                        switch (i)
                        {
                            case 0:
                                WeekBonusValue[0] -= 10;
                                break;

                            case 1:
                                WeekBonusValue[0] -= 20;
                                break;

                            case 2:
                                WeekBonusValue[0] -= 35;
                                break;

                            case 3:
                                WeekBonusValue[1] -= 10;
                                break;

                            case 4:
                                WeekBonusValue[1] -= 15;
                                break;

                            case 6:
                                WeekEventBoolTrigger[1] = false;
                                break;

                            case 7:
                                WeekEventBoolTrigger[2] = false;
                                break;
                        }

                        WeekEventBoolTrigger[0] = false;
                    }
                }
            }
        }
        if (WeekEventBoolTrigger[2]) Bgm.enabled = false;
        else Bgm.enabled = true;
        


        HourTimer();
    }

    private void Ending()
    {
        End.SetActive(true);
        Transform Im_Pos = End.transform.GetChild(1);
        Bgm.Stop();
        Im_Pos.GetChild(0).GetComponent<TextMeshProUGUI>().text = "총 지출액 : " + DM.UseGold[0];
        Im_Pos.GetChild(1).GetComponent<TextMeshProUGUI>().text = "총 매출액 : " + DM.BuyGold[0];
        Im_Pos.GetChild(2).GetComponent<TextMeshProUGUI>().text = "총 차액 : " + DataManager.Sign_PlusMinus(DM.BuyGold[0] - DM.UseGold[0]);
        Im_Pos.GetChild(3).GetComponent<TextMeshProUGUI>().text = "총 손님 방문 횟수 : " + DM.ComeCustomerCnt[0];
        Im_Pos.GetChild(4).GetComponent<TextMeshProUGUI>().text = "실수 횟수 : " + DM.MissCnt[0];
        Im_Pos.GetChild(5).GetComponent<TextMeshProUGUI>().text = "총 판매 횟수 : " + DM.SellCnt[0];
        Im_Pos.GetChild(6).GetComponent<TextMeshProUGUI>().text = "최종 보유 금액 : " + DM.HaveMoney;
        End.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(ScoreSet().ToString());
        End.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().SetText(ScoreSet().ToString());
        
        switch (ScoreSet())
        {
            case <= 300000:
                End.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("E");
                break;

            case < 500000:
                End.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("D");
                break;

            case < 1100000:
                End.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("C");
                break;

            case < 1800000:
                End.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("B");
                break;

            case < 2500000:
                End.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("A");
                break;

            case < 3000000:
                End.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("s");
                break;

            case >= 3000000:
                End.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("S");
                break;
        }
        #region 기록
        if (DataManager.GameDif == Diff.Easy || DataManager.GameDif == Diff.Hard || DataManager.GameDif == Diff.Normal)
        {
            string[] dataArr1 = PlayerPrefs.GetString("ScoreData").Split(',');
            int[] Best = { 0, 0, 0 };

            for (int i = 0; i < Best.Length; i++) Best[i] = System.Convert.ToInt32(dataArr1[i]);
            if (Best[(int)DataManager.GameDif] < ScoreSet())
            {
                Best[(int)DataManager.GameDif] = ScoreSet();

                string strArr1 = "";
                for (int i = 0; i < Best.Length; i++)
                {
                    strArr1 = strArr1 + Best[i];

                    if (i < Best.Length - 1)
                    {
                        strArr1 = strArr1 + ",";
                    }
                }

                PlayerPrefs.SetString("ScoreData", strArr1);
            }
        }
        if (PlayerPrefs.GetInt("HaveMoney") < DM.HaveMoney) PlayerPrefs.SetInt("HaveMoney", DM.HaveMoney);
        PlayerPrefs.SetInt("Customer", PlayerPrefs.GetInt("Customer") + DM.ComeCustomerCnt[0]);
        PlayerPrefs.SetInt("AllMiss", PlayerPrefs.GetInt("AllMiss") + DM.MissCnt[0]);
        if (PlayerPrefs.GetInt("Miss") < DM.MissCnt[0]) PlayerPrefs.SetInt("Miss", DM.MissCnt[0]);
        PlayerPrefs.SetInt("AllBuy", PlayerPrefs.GetInt("AllBuy") + DM.BuyCnt);
        if (PlayerPrefs.GetInt("Buy") < DM.BuyCnt) PlayerPrefs.SetInt("Buy", DM.BuyCnt);
        PlayerPrefs.SetInt("AllSell", PlayerPrefs.GetInt("AllSell") + DM.SellCnt[0]);
        if (PlayerPrefs.GetInt("Sell") < DM.SellCnt[0]) PlayerPrefs.SetInt("Sell", DM.SellCnt[0]);
        if (TitleManager.BestGetMoney[(int)DataManager.GameDif] < DM.BuyGold[0]) TitleManager.BestGetMoney[(int)DataManager.GameDif] = DM.BuyGold[0];
        #endregion
    }

    private int ScoreSet()
    {
        if (DataManager.GameDif == Diff.Event_1)
        {
            return DM.HaveMoney + DM.ComeCustomerCnt[2] * 10000 + (DM.SellCnt[2] - DM.MissCnt[2]) * 6666;
        }
        else
        {
            return DM.BuyGold[0] + DM.ComeCustomerCnt[0] * 8500 + (DM.SellCnt[0] - DM.MissCnt[0]) * 5000;
        }
        
    }

    internal void HourTimer()
    {
        if (DataManager.GameDif != Diff.Event_1)
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
        else
        {
            GetComponentInChildren<TextMeshProUGUI>().SetText((int)(1+ TimeData/10) + "일");
            if (TimeData/ 10 + 1 >= 31)
            {
                End.SetActive(true);
                DM.NowOpen = false;

                End.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().SetText(ScoreSet().ToString());
                switch (ScoreSet())
                {
                    case <= 300000:
                        End.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("E");
                        break;

                    case < 500000:
                        End.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("D");
                        break;

                    case < 1100000:
                        End.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("C");
                        break;

                    case < 1800000:
                        End.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("B");
                        break;

                    case < 2500000: //2,500,000
                        End.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("A");
                        break;

                    case < 3000000: // 3,000,000
                        End.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("s");
                        break;

                    case >= 3000000: //3,000,000
                        End.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("S");
                        break;
                }
                End.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("최종 매출액 : " + DM.HaveMoney);
                End.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().SetText("맞이한 손님 수 : " + DM.ComeCustomerCnt[2]);
                End.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().SetText("실수 횟수 : " + DM.MissCnt[2]);
                End.transform.GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>().SetText("판매한 물품 갯수 : " + DM.SellCnt[2]);
                End.transform.GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>().SetText("만족도 : " + (100 - (DM.MissCnt[2] / DM.ComeCustomerCnt[2]) * 100));
                End.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(ScoreSet().ToString());
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
