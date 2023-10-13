using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
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
