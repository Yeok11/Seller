using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Diff
{
    Easy,
    Normal,
    Hard,
    Event_1
}
public class DataManager : SingleTon<DataManager>
{
    static public Diff GameDif;

    internal int ItemTypeCount = 15;

    #region 아이템 설정
    [Header("아이템 종류")]
    internal string[] ItemType;

    [Header("아이템 갯수")]
    [SerializeField] internal int[] ItemCount;
    internal int[] ItemCount_Sell;

    [Header("아이템 가격")]
    internal int[] ItemPrice;
    #endregion

    [Header("돈")]
    [SerializeField] internal int HaveMoney = 0;
    internal int SellTotalMoney = 0;
    [SerializeField] TextMeshProUGUI MoneyPos;
    public AudioSource MoneySound;

    #region 시간 설정
    internal int Days = 0;
    internal List<string> Weeks = new List<string>();

    internal string[] OpCl = { "영업중", "영업준비", "영업종료" };
    internal bool NowOpen;
    internal int HourSec = 24;
    #endregion

    [Header("상점 아이템 정보")]
    [SerializeField] internal GameObject MarketBtsPos;
    internal int MarketItemCnt;

    internal string[,] EasyMessage = new string[10, 100];
    internal string[,] NormalMessage = new string[10, 100];


    internal List<int> MarketOrderData = new List<int>();
    internal List<string> CustomerOrderData = new List<string>();

    #region 인테리어 값 설정
    [Header("인테리어 능력치")]
    //시간 / 방문률 / 매입가 / 판매가
    internal int[] InteriorLevel = { 1, 1, 1, 1 };
    internal int[,] NextCost = {{ 10000, 15000, 12000, 10000 }, { 12000, 17000, 15000, 12000 }, { 10000, 15000, 12000, 10000 }, { 10000, 15000, 12000, 10000 }, { 10000, 15000, 12000, 10000 } };
    internal int[,] BonusPer = new int[4, 6] { { 0, 3, 5, 7, 10, 0 } , { 0, 10, 25, 35, 50, 0 } , { 0, 3, 7, 15, 20, 0 } , { 0, 5, 7, 12, 20, 0 } };
    #endregion

    internal int MaxDay = 50;
    internal int[,] OrderCntPer = new int[10, 4] { { 40, 10, 0, 0 }, { 30, 15, 5, 0 }, { 20, 25, 5, 0 }, { 15, 25, 10, 0 }, { 10, 25, 10, 10 }, { 5, 25, 10, 5 }, { 0, 20, 20, 10 }, { 0, 10, 25, 15 }, { 0, 0, 30, 20 }, { 0, 0, 20, 30 } };


    internal string[] CntMes = new string[] { "개 정도면 될 거 같아요.", "개 내놔라.", "개만 주세요.", "개만 줘.", "번의 은혜로 내려주시길.", "개 정도 내놓아 보거라."};

    //주간 리포트 기록 데이터
    // 0: 모든 값 토탈 __ 1: 지난 주 값 __ 2: 이번 주 값
    internal List<int> UseGold = new List<int>(3) { 0, 0, 0 };

    internal List<int> BuyGold = new List<int>(3) { 0, 0, 0 };

    internal List<int> SellCnt = new List<int>(3) { 0, 0, 0 };
    internal List<int> ComeCustomerCnt = new List<int>(3) { 0, 0, 0 };
    internal List<int> MissCnt = new List<int>(3) { 0, 0, 0 };

    private void Awake()
    {
        ItemType = new string[ItemTypeCount];
        ItemCount = new int[ItemTypeCount];
        ItemCount_Sell = new int[ItemTypeCount];
        ItemPrice = new int[ItemTypeCount];

        Title.SizeCtrl();
    }

    private void Start()
    {
        HaveMoney = 10000;
    }

    private void Update()
    {
        MoneyPos.text = HaveMoney.ToString();
    }

    internal void DataInput()
    {
        for (int i = 0; i < ItemTypeCount; i++)
        {
            ItemType[i] = CSVManager.Instance.csvdata.ItemData[i]["ItemName"].ToString();
            ItemPrice[i] = (int)CSVManager.Instance.csvdata.ItemData[i]["ItemPrice"];
        }

        //손님 주문 배열 정리
        for (int i = 0; i < CSVManager.Instance.csvdata.CustomMessage.Count; i++)
        {
            EasyMessage[(int)CSVManager.Instance.csvdata.EasyCustomMessage[i]["Num"] / 10, (int)CSVManager.Instance.csvdata.EasyCustomMessage[i]["Num"] % 10 - 1]
                = CSVManager.Instance.csvdata.EasyCustomMessage[i]["Mes1"].ToString();

            NormalMessage[(int)CSVManager.Instance.csvdata.CustomMessage[i]["Num"] / 10, (int)CSVManager.Instance.csvdata.CustomMessage[i]["Num"] % 10 - 1] 
                = CSVManager.Instance.csvdata.CustomMessage[i]["Mes1"].ToString();

            //확인
            //Debug.Log(EasyMessage + " " + (int)CSVManager.Instance.csvdata.EasyCustomMessage[i]["Num"] / 10 + "" + ((int)CSVManager.Instance.csvdata.EasyCustomMessage[i]["Num"] % 10 - 1).ToString());
        }
    }
}
