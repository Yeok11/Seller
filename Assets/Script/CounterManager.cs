using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

[HideInInspector]
public class CounterManager : SingleTon<CounterManager>
{
    private GameObject Shelf;
    Transform SellPos;

    private GameObject[] Slot = new GameObject[15];

    [SerializeField] private TextMeshProUGUI SellData;
    [SerializeField] private TextMeshProUGUI TotalSign;

    private TextMeshProUGUI[] SellDatas = new TextMeshProUGUI[15];

    DataManager DM;

    List<string> ItemSellDatas = new List<string>();

    private void Start()
    {
        DM = DataManager.Instance;

        Shelf = GameObject.Find("Shelf_Contants");
        SellPos = GameObject.Find("Sell_Contants").transform;

        for (int i = 0; i < Shelf.transform.childCount; i++)
        {
            Slot[i] = Shelf.transform.GetChild(i).gameObject;

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
                
                SellDatas[NullDataPos].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = DM.ItemCount_Sell[CodeNum].ToString();

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

    ReSet:
        for (int j = 0; j < DM.CustomerOrderData.Count; j++)
        {
            if (ItemSellDatas.Count() == 0 || DM.CustomerOrderData.Count() == 0)
            {
                break;
            }
            

            Debug.Log("�Ǹ� ���� : " + ItemSellDatas[i] + " / �մ��� �ֹ��� ����" + DM.CustomerOrderData[j]);

            if (ItemSellDatas[i] == DM.CustomerOrderData[j])
            {
                if ((ItemSellDatas.FindAll(a => a.Contains(ItemSellDatas[i]))).Count == (DM.CustomerOrderData.FindAll(b => b.Contains(DM.CustomerOrderData[j]))).Count)
                {
                    Debug.Log("���� ��ġ : " + ItemSellDatas[i]);
                    SellAndGetMoney(1, i);
                }
                else if ((ItemSellDatas.FindAll(a => a.Contains(ItemSellDatas[i]))).Count > (DM.CustomerOrderData.FindAll(b => b.Contains(DM.CustomerOrderData[j]))).Count)
                {
                    Debug.Log("�ֹ� �������� �����ϴ�. : " + ItemSellDatas[i]);
                    SellAndGetMoney(0.7f, i);
                }
                else if ((ItemSellDatas.FindAll(a => a.Contains(ItemSellDatas[i]))).Count < (DM.CustomerOrderData.FindAll(b => b.Contains(DM.CustomerOrderData[j]))).Count)
                {
                    Debug.Log("�ֹ� �������� �����ϴ�. : " + ItemSellDatas[i]);
                    SellAndGetMoney(0.7f, i);
                }
                
                ItemSellDatas.RemoveAt(i);
                DM.CustomerOrderData.RemoveAt(j);

                goto ReSet;
            }
        }

        if (i < AG - i)
        {
            i++;
            goto ReSet;
        }

        ItemSellDatas.Clear();
        DM.CustomerOrderData.Clear();
    }

    private void SellAndGetMoney(float per, int ItemCode)
    {
        for (int i = 0; i < DM.ItemType.Length; i++)
        {
            if (ItemSellDatas[ItemCode] == DM.ItemType[i])
            {
                DM.HaveMoney += (int)(DM.ItemPrice[i] * per);
                Debug.Log($"+{(int)(DM.ItemPrice[i] * per)} ��ŭ ��带 ȹ���ϼ̽��ϴ�.");
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

        ItemSellDatas.Clear();

        Debug.Log("������ �ǸŰ� ��� �Ǽ̽��ϴ�.");

        ShelfReset();
        MonitorReset();
    }

    public void Monitor_Sell()
    {
        if (CustomerManager.Instance.OrderNowDo == false)
        {
            Debug.Log("������ �ǸŰ� ���� �ϼ̽��ϴ�.");

            JudgeSell_Items();
            MonitorReset();
            CustomerManager.Instance.ResetCustomer();
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
