using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;

[HideInInspector]
public class CounterManager : SingleTon<CounterManager>
{
    private GameObject Shelf;
    private Transform SellPos;

    private GameObject[] Slot = new GameObject[15];

    //internal int WeekSellBonus;


    [SerializeField] private TextMeshProUGUI SellData;
    [SerializeField] private TextMeshProUGUI TotalSign;

    private TextMeshProUGUI[] SellDatas = new TextMeshProUGUI[15];

    private DataManager DM;

    private List<string> ItemSellDatas = new List<string>();

    
    private void Start()
    {
        DM = DataManager.Instance;

        Shelf = GameObject.Find("Shelf_Contants");
        SellPos = GameObject.Find("Sell_Contants").transform;

        for (int i = 0; i < Shelf.transform.childCount; i++)
        {
            Slot[i] = Shelf.transform.GetChild(i).gameObject;

            DM.ItemCount[i] += 15;

            Slot[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = DM.ItemCount[i].ToString();
            Slot[i].transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = DM.ItemType[i];

            SellDatas[i] = Instantiate(SellData, SellPos);
        }

        MonitorReset();
    }

    public void ShelfItem_Click(int CodeNum)
    {
        int NullDataPos;
        //���Ը� ������ �� Ȯ��
        if (DM.NowOpen == true)
        {
            //���� ���� Ȯ��
            if (DM.ItemCount[CodeNum] > 0)
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

                //���� ����
                DM.ItemCount_Sell[CodeNum]++;
                DM.ItemCount[CodeNum]--;
                
                SellDatas[NullDataPos].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = DM.ItemCount_Sell[CodeNum].ToString();

                //���� ����
                DM.SellTotalMoney += DM.ItemPrice[CodeNum];
                TotalSign.text = DM.SellTotalMoney + "��";

                //�Ǵ��� ���� ����Ʈ�� ���� ����
                ItemSellDatas.Add(SellDatas[NullDataPos].text);

                //���� ǥ��
                ShelfReset();
            }
            else Debug.Log("�� �̻� ������ �����ϴ�.");
        }
        else Debug.Log("���Ը� ���� ���� �ʾҽ��ϴ�.");
    }

    private void JudgeSell_Items()
    {
        int i = 0;
        int AG = ItemSellDatas.Count();

        bool mistake = false;

        if (ItemSellDatas.Count() == 0) mistake = true;

    ReSet:
        for (int j = 0; j < DM.CustomerOrderData.Count;j++)
        {
            if (ItemSellDatas.Count() == 0 || DM.CustomerOrderData.Count() == 0)
            {
                break;
            }

            Debug.Log(i + " / " + j);
            Debug.Log("�Ǹ� ���� : " + ItemSellDatas[i] + " / �մ��� �ֹ��� ����" + DM.CustomerOrderData[j]);

            if (ItemSellDatas[i] == DM.CustomerOrderData[j])
            {
                if ((ItemSellDatas.FindAll(a => a.Contains(ItemSellDatas[i]))).Count == (DM.CustomerOrderData.FindAll(b => b.Contains(DM.CustomerOrderData[j]))).Count)
                {
                    //Debug.Log("���� ��ġ : " + ItemSellDatas[i]);
                    SellAndGetMoney(1, i);
                }
                else if ((ItemSellDatas.FindAll(a => a.Contains(ItemSellDatas[i]))).Count > (DM.CustomerOrderData.FindAll(b => b.Contains(DM.CustomerOrderData[j]))).Count || ItemSellDatas.FindAll(a => a.Contains(ItemSellDatas[i])).Count < (DM.CustomerOrderData.FindAll(b => b.Contains(DM.CustomerOrderData[j]))).Count)
                {
                    //Debug.Log("�ֹ� �������� �����ϴ�. : " + ItemSellDatas[i]);
                    SellAndGetMoney(0.7f, i);
                    mistake = true;
                }


                DM.SellCnt[2]++;
                ItemSellDatas.RemoveAt(i);
                DM.CustomerOrderData.RemoveAt(j);
                j = 0;

                goto ReSet;
            }
            else mistake = true;
        }

        //if (mistake) DM.MissCnt[2]++;

    /*
        if (i < AG - i)
        {
            i++;
            goto ReSet;
        }
    */

        ItemSellDatas.Clear();
        DM.CustomerOrderData.Clear();
        CustomerManager.Instance.ResetCustomer(mistake);
    }

    private void SellAndGetMoney(float per, int ItemCode)
    {
        for (int i = 0; i < DM.ItemType.Length; i++)
        {
            if (ItemSellDatas[ItemCode] == DM.ItemType[i])
            {
                int plus = (int)(DM.ItemPrice[i] * per * DM.BonusPer[3,DM.InteriorLevel[3] - 1] / 100);
                int totalSellCost = (int)(DM.ItemPrice[i] * per) + plus;
                totalSellCost += TimeManager.Instance.WeekBonusValue[0] == 0 ? 0 : totalSellCost/TimeManager.Instance.WeekBonusValue[0];
                //WeekSellBonus = (int a) => totalSellCost += (totalSellCost / a);

                //WeekBonu(TimeManager.Instance.MoneyTrigger_Weekevent);
                DM.HaveMoney += totalSellCost;
                //Debug.Log($"+{(int)(DM.ItemPrice[i] * per)}(+{plus}) ��ŭ ��带 ȹ���ϼ̽��ϴ�.");
                DM.BuyGold[2] += (int)(DM.ItemPrice[i] * per) + plus;
                break;
            }
        }
    }

    internal void AddItem()
    {
        int j = DM.MarketOrderData.Count;

        for (int i = 0; i < j; i++)
        {
            DM.ItemCount[DM.MarketOrderData[0]]++;
            DM.MarketOrderData.RemoveAt(0);
        }

        Debug.Log("������ ��� �Ǿ����ϴ�.");

        ShelfReset();
    }

    internal void ShelfReset()
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

        ItemSellDatas.Clear();

        //Debug.Log("������ �ǸŰ� ��� �Ǽ̽��ϴ�.");

        ShelfReset();
        MonitorReset();
    }

    public void Monitor_Sell()
    {
        if (CustomerManager.Instance.OrderNowDo == false && CustomerManager.Instance.CanSell == true)
        {
            Debug.Log("������ �ǸŰ� ���� �ϼ̽��ϴ�.");

            if (DataManager.GameDif == Diff.Event_1)
            {
                for (int i = 0; i < DM.ItemCount.Length; i++) DM.ItemCount[i] = 99;
                ShelfReset();
            }


            AudioManager.Instance.audiosource[3].Play();

            JudgeSell_Items();
            StopAllCoroutines();
            MonitorReset();
            CustomerManager.Instance.CanSell = false;
        }
    }

    private void MonitorReset()
    {
        DM.SellTotalMoney = 0;
        TotalSign.text = DM.SellTotalMoney + "��";

        for (int i = 0; i < SellPos.childCount; i++)
        {
            DM.ItemCount_Sell[i] = 0;

            if (SellDatas[i].gameObject.activeSelf == true) SellDatas[i].gameObject.SetActive(false);
        }
    }
}
