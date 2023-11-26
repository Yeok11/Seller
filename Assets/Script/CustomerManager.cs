using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class CustomerManager : SingleTon<CustomerManager>
{
    GameObject NowCustomer;
    [SerializeField] Transform CustomerOrderPos;
    internal bool OrderNowDo;

    int CustomerType;
    internal float del;
    int Gender;
    internal bool CanSell = false;

    //�մ� ���� �ð� ����
    internal IEnumerator SpawnDelay()
    {
        //SpawnDelay = StartCoroutine(SpawnDelay());

        int i = DataManager.Instance.BonusPer[1, DataManager.Instance.InteriorLevel[1]];
        Gender = Random.Range(0, 2);

        del = Random.Range(1f - i / (100 + i), 4f - i / (100 + i));

        Debug.Log(del);

        yield return new WaitForSeconds(del);
        OrderNowDo = true;

        if (DataManager.Instance.NowOpen == true && OrderNowDo == true && del != 0) SpawnCustomer();
    }

    private void AllCustomerCut()
    {
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
            transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
        }
    }

    //�մ� ��ȯ
    private void SpawnCustomer()
    {
        AllCustomerCut();

        NowCustomer = transform.GetChild(RandomCustomer()).gameObject;
        NowCustomer.SetActive(true);
        NowCustomer.transform.GetChild(Gender).gameObject.SetActive(true);
        //Debug.Log(NowCustomer.transform.GetChild(Gender).gameObject);
        DataManager.Instance.ComeCustomerCnt[2]++;

        StartCoroutine(CustomerOrder());
    }

    //�մ� ���� ����
    private int RandomCustomer()
    {
        CustomerType = Random.Range(1, 14);

        //������
        if (DataManager.Instance.Days >= 7) CustomerType = Random.Range(1, 18);
        //������
        if (DataManager.Instance.Days >= 15) CustomerType = Random.Range(1, 20);
        //����
        if (DataManager.Instance.Days >= 40 ||DataManager.GameDif == Diff.Event_1) CustomerType = Random.Range(1, 21);
        BACK:

        switch (CustomerType)
        {
            case <= 8:
                if (TimeManager.Instance.WeekEventBoolTrigger[1]) goto BACK;
                return CustomerType = 0;
            case <= 11:
                return CustomerType = 1;
            case <= 14:
                return CustomerType = 2;
            case <= 17:
                return CustomerType = 3;
            case <= 19:
                return CustomerType = 4;
            case <= 20:
                if (TimeManager.Instance.WeekEventBoolTrigger[1]) goto BACK;
                return CustomerType = 5;

            //����
            default: return 100;
        }
    }

    //�ֹ� ����
    internal IEnumerator CustomerOrder()
    {
        yield return new WaitForSeconds(0.25f);
        
        if (OrderNowDo == true && del != 0)
        {
            CustomerOrderPos.GetChild(0).gameObject.SetActive(true);
            CustomerOrderPos.GetChild(1).gameObject.SetActive(true);
            OrderMessage();
            //DataManager.Instance.ComeCustomerCnt[2] += 1;
            OrderNowDo = false;
            CanSell = true;
        }
    }

    //�ֹ� ����
    private void OrderMessage()
    {
        //���� ���� ���� ����
        int OrderCnt = Random.Range(0,100);

        #region ��¥ ��� Ȯ���� ��ȯ
        int BonusValue1 = DataManager.Instance.OrderCntPer[DataManager.Instance.Days / (DataManager.Instance.MaxDay / 10), 0];
        int BonusValue2 = BonusValue1 + DataManager.Instance.OrderCntPer[DataManager.Instance.Days / (DataManager.Instance.MaxDay / 10), 1];
        int BonusValue3 = BonusValue2 + DataManager.Instance.OrderCntPer[DataManager.Instance.Days / (DataManager.Instance.MaxDay / 10), 2];
        int BonusValue4 = BonusValue3 + DataManager.Instance.OrderCntPer[DataManager.Instance.Days / (DataManager.Instance.MaxDay / 10), 3];

        if (OrderCnt < 20 + BonusValue1) OrderCnt = 1;
        else if (OrderCnt < 35 + BonusValue2) OrderCnt = 2;
        else if (OrderCnt < 45 + BonusValue3) OrderCnt = 3;
        else if (OrderCnt < 50 + BonusValue4) OrderCnt = 4;
        #endregion

        List<int> AgainTime = new List<int>(OrderCnt);

        #region �ֹ�
        int i = 0;
    More:
        int RangeValue = FindRange();
        
        //�̹� �ִ� �� Ȯ���ϰ� �ִٸ� �ٽ� ����
        for (int j = 0; j < AgainTime.Count; j++) if (RangeValue == AgainTime[j]) goto More;

        //Debug.Log("�մ� ������ : " + CustomerType);
        // ���� ����
        if (CustomerType < 5)
        {
            //���� ����
            int objCnt = Random.Range(0,100);
            //Debug.Log("���� : " + objCnt + " / ������ ǰ�� : " + RangeValue);

            if (objCnt < 20 + BonusValue1) objCnt = 1;
            else if (objCnt < 30 + BonusValue2) objCnt = 2;
            else if (objCnt < 40 + BonusValue3) objCnt = 3;
            else if (objCnt < 45 + BonusValue4) objCnt = 4;
            else if (objCnt < 50 + BonusValue4) objCnt = 5;


            for (int j = 0; j < objCnt; j++)
            {
                //����Ʈ�� �� �ֱ�
                AgainTime.Add(RangeValue);

                //����Ʈ�� �� �ֱ� 2
                if (RangeValue < 6) DataManager.Instance.CustomerOrderData.Add(CSVManager.Instance.csvdata.ItemData[RangeValue + 9]["ItemName"].ToString());
                else
                {
                    switch (CustomerType)
                    {
                        //����
                        case 1:
                            if (RangeValue == 6) DataManager.Instance.CustomerOrderData.Add(CSVManager.Instance.csvdata.ItemData[0]["ItemName"].ToString());
                            else if (RangeValue == 7) DataManager.Instance.CustomerOrderData.Add(CSVManager.Instance.csvdata.ItemData[1]["ItemName"].ToString());
                            else if (RangeValue == 8) DataManager.Instance.CustomerOrderData.Add(CSVManager.Instance.csvdata.ItemData[2]["ItemName"].ToString());
                            break;
                        //�ü�
                        case 2:
                            if (RangeValue == 6) DataManager.Instance.CustomerOrderData.Add(CSVManager.Instance.csvdata.ItemData[7]["ItemName"].ToString());
                            else if (RangeValue == 7) DataManager.Instance.CustomerOrderData.Add(CSVManager.Instance.csvdata.ItemData[6]["ItemName"].ToString());
                            else if (RangeValue == 8) DataManager.Instance.CustomerOrderData.Add(CSVManager.Instance.csvdata.ItemData[8]["ItemName"].ToString());
                            break;
                        //������
                        case 3:
                            if (RangeValue == 6) DataManager.Instance.CustomerOrderData.Add(CSVManager.Instance.csvdata.ItemData[7]["ItemName"].ToString());
                            else if (RangeValue == 7) DataManager.Instance.CustomerOrderData.Add(CSVManager.Instance.csvdata.ItemData[6]["ItemName"].ToString());
                            else if (RangeValue == 8) DataManager.Instance.CustomerOrderData.Add(CSVManager.Instance.csvdata.ItemData[5]["ItemName"].ToString());
                            break;
                        //������
                        case 4:
                            if (RangeValue == 6) DataManager.Instance.CustomerOrderData.Add(CSVManager.Instance.csvdata.ItemData[3]["ItemName"].ToString());
                            else if (RangeValue == 7) DataManager.Instance.CustomerOrderData.Add(CSVManager.Instance.csvdata.ItemData[4]["ItemName"].ToString());
                            else if (RangeValue == 8) DataManager.Instance.CustomerOrderData.Add(CSVManager.Instance.csvdata.ItemData[2]["ItemName"].ToString());
                            break;

                        default:
                            break;
                    }
                }
            }
        }
        else //������ ���� 10�� ����
        {
            for (int j = 0; j < 10; j++)
            {
                AgainTime.Add(RangeValue);

                DataManager.Instance.CustomerOrderData.Add(CSVManager.Instance.csvdata.ItemData[RangeValue + 9]["ItemName"].ToString());
            }
        }

        i++;
        if (i < OrderCnt) goto More;

        // �޽��� �ʱ�ȭ
        CustomerOrderPos.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";

        //���������� �ߺ� ���� �� �� �ϳ������� ���
        List<int> textContents = AgainTime.Intersect(AgainTime).ToList();

        //�޽��� ����
        for (i = 0; i < textContents.Count(); i++)
        {
            string TextContents = "";

            int OrdCnt = FindItemCnt(AgainTime);

            switch (DataManager.GameDif)
            {
                case Diff.Easy:
                    TextContents = DataManager.Instance.EasyMessage[CustomerType, textContents[i]];
                    break;

                case Diff.Normal:
                    if (Random.Range(0, 100) < 20 + DataManager.Instance.Days)
                        TextContents = DataManager.Instance.NormalMessage[CustomerType, textContents[i]];
                    else TextContents = DataManager.Instance.EasyMessage[CustomerType, textContents[i]];

                    break;

                case Diff.Hard:
                    if (Random.Range(0, 100) < 60 + DataManager.Instance.Days)
                        TextContents = DataManager.Instance.NormalMessage[CustomerType, textContents[i]];
                    else TextContents = DataManager.Instance.EasyMessage[CustomerType, textContents[i]];
                    break;

                case Diff.Event_1:
                    if (Random.Range(0, 100) < 20)
                        TextContents = DataManager.Instance.NormalMessage[CustomerType, textContents[i]];
                    else TextContents = DataManager.Instance.EasyMessage[CustomerType, textContents[i]];
                    break;
            }

            CustomerOrderPos.GetChild(1).GetComponent<TextMeshProUGUI>().text += TextContents + OrdCnt + DataManager.Instance.CntMes[CustomerType] + "\n";
            AgainTime.RemoveRange(0,OrdCnt);
        }
        #endregion
    }

    int FindItemCnt(List<int> _sources)
    {
        _sources.Add(99);
        for (int i = 0; i < _sources.Count; i++)
        {
            if (_sources.First() != _sources[i])
            {
                return i--;
            }
        }
        return 99;
    }

    private int FindRange()
    {
        int i;
        for (i = 0; i < DataManager.Instance.NormalMessage.GetLength(0); i++)
            if (DataManager.Instance.NormalMessage[CustomerType, i] == null) break;
            
    back:
        int j = Random.Range(0, i);

        if (DataManager.Instance.NormalMessage[CustomerType,j] == "null") goto back;

        return j;
    }
    
    //�մ� ����
    internal void ResetCustomer(bool miss)
    {
        if (NowCustomer != null)
        {
            NowCustomer.SetActive(false);
            if (miss == true) DataManager.Instance.MissCnt[2]++;
        }

        NowCustomer = null;

        CustomerOrderPos.GetChild(0).gameObject.SetActive(false);
        CustomerOrderPos.GetChild(1).gameObject.SetActive(false);

        if (CustomerOrderPos.GetChild(1).gameObject.activeSelf == true) CustomerOrderPos.gameObject.SetActive(false);

        if (DataManager.Instance.NowOpen == true) StartCoroutine(SpawnDelay());
    }
}
