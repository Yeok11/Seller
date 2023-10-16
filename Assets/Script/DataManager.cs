using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingleTon<DataManager>
{
    internal int ItemTypeCount = 15;

    [Header("아이템 종류")]
    internal string[] ItemType;

    [Header("아이템 갯수")]
    [SerializeField]internal int[] ItemCount;
    internal int[] ItemCount_Sell;

    [Header("아이템 가격")]
    internal int[] ItemPrice;

    [Header("돈")]
    internal int HaveMoney = 0;
    internal int SellTotalMoney = 0;

    internal int Days = 0;
    internal List<string> Weeks = new List<string>();

    internal string[] OpCl = { "OPEN", "CLOSE", "다음 날" };
    internal bool NowOpen;
    internal int HourSec = 24;

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

    private void DataInput()
    {
        for (int i = 0; i < ItemTypeCount; i++)
        {
            ItemType[i] = CSVManager.Instance.csvdata.ItemData[i]["ItemName"].ToString();
            ItemPrice[i] = (int)CSVManager.Instance.csvdata.ItemData[i]["ItemPrice"];
        }
    }
}
