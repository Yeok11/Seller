using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DataManager : SingleTon<DataManager>
{
    internal int ItemTypeCount = 15;

    [Header("������ ����")]
    internal string[] ItemType;

    [Header("������ ����")]
    [SerializeField]internal int[] ItemCount;
    internal int[] ItemCount_Sell;

    [Header("������ ����")]
    internal int[] ItemPrice;

    [Header("��")]
    internal int HaveMoney = 0;
    internal int SellTotalMoney = 0;
    [SerializeField] TextMeshProUGUI MoneyPos;

    internal int Days = 0;
    internal List<string> Weeks = new List<string>();

    internal string[] OpCl = { "OPEN", "CLOSE", "���� ��", "���� Ȯ��" };
    internal bool NowOpen;
    internal int HourSec = 24;

    [Header("���� ������ ����")]
    [SerializeField] internal GameObject MarketBtsPos;
    internal int MarketItemCnt;

    internal List<int> OrderData = new List<int>();

    private void Awake()
    {
        ItemType = new string[ItemTypeCount];
        ItemCount = new int[ItemTypeCount];
        ItemCount_Sell = new int[ItemTypeCount];
        ItemPrice = new int[ItemTypeCount];
    }

    private void Start()
    {
        DataInput();
    }

    private void Update()
    {
        MoneyPos.text = HaveMoney.ToString();
    }

    private void DataInput()
    {
        for (int i = 0; i < ItemTypeCount; i++)
        {
            ItemType[i] = CSVManager.Instance.csvdata.ItemData[i]["ItemName"].ToString();
            ItemPrice[i] = (int)CSVManager.Instance.csvdata.ItemData[i]["ItemPrice"];
        }
    }
}
