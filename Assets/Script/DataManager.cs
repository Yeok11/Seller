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
    [SerializeField] internal int HaveMoney = 999999;
    internal int SellTotalMoney = 0;
    [SerializeField] TextMeshProUGUI MoneyPos;

    internal int Days = 0;
    internal List<string> Weeks = new List<string>();

    internal string[] OpCl = { "������", "�����غ�", "��������" };
    internal bool NowOpen;
    internal int HourSec = 24;

    [Header("���� ������ ����")]
    [SerializeField] internal GameObject MarketBtsPos;
    internal int MarketItemCnt;

    internal List<int> MarketOrderData = new List<int>();

    internal string[,] Message = new string[10, 100];

    internal List<string> CustomerOrderData = new List<string>();


    [Header("���׸��� �ɷ�ġ")]
    //�ð� / �湮�� / ���԰� / �ǸŰ�
    internal int[] InteriorLevel = { 1, 1, 1, 1 };
    internal int[] NextCost = { 10000, 15000, 12000, 10000 };
    internal int[,] BonusPer = new int[4, 6] { { 0, 3, 5, 7, 10, 0 } , { 0, 10, 25, 35, 50, 0 } , { 0, 3, 7, 15, 20, 0 } , { 0, 5, 7, 12, 20, 0 } };

    private void Awake()
    {
        ItemType = new string[ItemTypeCount];
        ItemCount = new int[ItemTypeCount];
        ItemCount_Sell = new int[ItemTypeCount];
        ItemPrice = new int[ItemTypeCount];
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

        for (int i = 0; i < CSVManager.Instance.csvdata.CustomMessage.Count; i++)
        {
            Message[(int)CSVManager.Instance.csvdata.CustomMessage[i]["Num"] / 10, (int)CSVManager.Instance.csvdata.CustomMessage[i]["Num"] % 10 - 1] 
                = CSVManager.Instance.csvdata.CustomMessage[i]["Message"].ToString();

            //Debug.Log(Message + " " + (int)CSVManager.Instance.csvdata.CustomMessage[i]["Num"] / 10 + "" + ((int)CSVManager.Instance.csvdata.CustomMessage[i]["Num"] % 10 - 1).ToString());
        }
    }
}
