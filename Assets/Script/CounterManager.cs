using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[HideInInspector]
public class CounterManager : MonoBehaviour
{
    private GameObject Shelf;
    private GameObject[] Slot = new GameObject[15];

    [SerializeField] private TextMeshProUGUI SellData;
    [SerializeField] private TextMeshProUGUI TotalSign;
    private TextMeshProUGUI[] SellDatas = new TextMeshProUGUI[15];
    GameObject SellPos;  

    DataManager DM;
    CustomerManager CM;

    void Start()
    {
        DM = GetComponent<DataManager>();
        CM = GameObject.Find("CustomerManager").GetComponent<CustomerManager>();

        Shelf = GameObject.Find("Shelf_Contants");
        SellPos = GameObject.Find("Sell_Contants");

        for (int i = 0; i < Shelf.transform.childCount; i++)
        {
            Slot[i] = Shelf.transform.GetChild(i).gameObject;

            Slot[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = DM.ItemCount[i].ToString();
            Slot[i].transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = DM.ItemPrice[i].ToString();

            SellDatas[i] = Instantiate(SellData, SellPos.transform);
        }

        MonitorReset();
    }

    public void ShelfItem_Click(int CodeNum)
    {
        int NullDataPos;
        
        //���� ���� Ȯ��
        if(DM.ItemCount[CodeNum] > 0)
        {
            //���� �� & �� �� Ȯ��
            for (NullDataPos = 0; NullDataPos < SellDatas.Length; NullDataPos++)
            {
                if (SellDatas[NullDataPos].gameObject.activeSelf == false) break;
                else if (SellDatas[NullDataPos].text == CSVManager.Instance.csvdata.ItemData[CodeNum]["ItemName"].ToString()) goto ShelfItem_Click_Skip1;
            }

            SellDatas[NullDataPos].gameObject.SetActive(true);
            SellDatas[NullDataPos].text = CSVManager.Instance.csvdata.ItemData[CodeNum]["ItemName"].ToString();

            ShelfItem_Click_Skip1:

            DM.ItemCount_Sell[CodeNum]++;
            DM.ItemCount[CodeNum]--;
            SellDatas[NullDataPos].transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = DM.ItemCount_Sell[CodeNum].ToString();

            DM.SellTotalMoney += DM.ItemPrice[CodeNum];
            TotalSign.text = DM.SellTotalMoney + "��";
            
            ShelfReset();
        }
        else
        {
            Debug.Log("�� �̻� ������ �����ϴ�.");
        }
    }

    private void ShelfReset()
    {
        for (int i = 0; i < Slot.Length; i++)
        {
            Slot[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = DM.ItemCount[i].ToString();
        }
    }

    public void Monitor_Cancel()
    {
        for (int i = 0; i < DM.ItemTypeCount; i++)
        {
            DM.ItemCount[i] += DM.ItemCount_Sell[i];
        }

        Debug.Log("������ �ǸŰ� ��� �Ǽ̽��ϴ�.");

        ShelfReset();
        MonitorReset();
    }

    public void Monitor_Sell()
    {
        Debug.Log("������ �ǸŰ� ���� �ϼ̽��ϴ�.");

        MonitorReset();
        CM.ResetCustomer();
    }

    private void MonitorReset()
    {
        DM.SellTotalMoney = 0;
        TotalSign.text = DM.SellTotalMoney + "��";

        for (int i = 0; i < SellPos.transform.childCount; i++)
        {
            DM.ItemCount_Sell[i] = 0;

            if (SellDatas[i].gameObject.activeSelf == true) SellDatas[i].gameObject.SetActive(false);
        }
    }

    void Error()
    {
        Debug.Log("Error");
    }
}
