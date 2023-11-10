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


    //�մ� ���� �ð� ����
    internal IEnumerator SpawnDelay()
    {
        int i = DataManager.Instance.BonusPer[1, DataManager.Instance.InteriorLevel[1]];

        float del = Random.Range(1f - i / (100 + i), 4f - i / (100 + i));

        Debug.Log(del);
        OrderNowDo = true;

        yield return new WaitForSeconds(del);

        if (DataManager.Instance.NowOpen == true) SpawnCustomer();
    }

    //�մ� ��ȯ
    internal void SpawnCustomer()
    {
        NowCustomer = transform.GetChild(RandomCustomer()).gameObject;
        NowCustomer.SetActive(true);
        NowCustomer.transform.GetChild(Random.Range(0, 2)).gameObject.SetActive(true);

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
        if (DataManager.Instance.Days >= 40) CustomerType = Random.Range(1, 21);

        switch (CustomerType)
        {
            case <= 8:
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
                return CustomerType = 5;

            //����
            default: return 100;
        }
    }

    //�ֹ� ����
    private IEnumerator CustomerOrder()
    {
        yield return new WaitForSeconds(0.25f);

        CustomerOrderPos.GetChild(0).gameObject.SetActive(true);

        yield return new WaitForSeconds(0.15f);

        if (OrderNowDo == true)
        {
            CustomerOrderPos.GetChild(1).gameObject.SetActive(true);
            OrderMessage();
        }

        OrderNowDo = false;
    }

    //�ֹ� ����
    private void OrderMessage()
    {
        //���� ���� ���� ����
        int OrderCnt = Random.Range(0,100);

        #region ��¥ ��� Ȯ���� ��ȯ
        int BonusValue1 = DataManager.Instance.OrderCntPer[DataManager.Instance.Days, 0];
        int BonusValue2 = BonusValue1 + DataManager.Instance.OrderCntPer[DataManager.Instance.Days, 1];
        int BonusValue3 = BonusValue2 + DataManager.Instance.OrderCntPer[DataManager.Instance.Days, 2];
        int BonusValue4 = BonusValue3 + DataManager.Instance.OrderCntPer[DataManager.Instance.Days, 3];

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
        for (int j = 0; j < AgainTime.Count - 1; j++)
        {
            if (RangeValue == AgainTime[j]) goto More;
        }

        Debug.Log("�մ� ������ : " + CustomerType);
        // ���� ����
        if (CustomerType < 5)
        {
            int objCnt = Random.Range(1,6);
            Debug.Log("���� : " + objCnt + " / ������ : " + RangeValue);

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
        else
        {

        }

        i++;
        if (i < OrderCnt) goto More;

        // �޽��� �ʱ�ȭ
        CustomerOrderPos.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";

        List<int> textContents = AgainTime.Intersect(AgainTime).ToList();

        //�޽��� ����
        for (i = 0; i < textContents.Count(); i++)
        {
            string TextContents = DataManager.Instance.Message[CustomerType, textContents[i]];
            
            CustomerOrderPos.GetChild(1).GetComponent<TextMeshProUGUI>().text += TextContents + " ";
        }
        #endregion
    }

    private int FindRange()
    {
        int i;
        for (i = 0; i < DataManager.Instance.Message.GetLength(0); i++)
            if (DataManager.Instance.Message[CustomerType, i] == null) break;
            
    back:

        int j = Random.Range(0, i);

        if (DataManager.Instance.Message[CustomerType,j] == "null") goto back;

        return j;
    }
    
    //�մ� ����
    internal void ResetCustomer()
    {
        if (NowCustomer != null) NowCustomer.SetActive(false);

        CustomerOrderPos.GetChild(0).gameObject.SetActive(false);
        CustomerOrderPos.GetChild(1).gameObject.SetActive(false);

        if (CustomerOrderPos.GetChild(1).gameObject.activeSelf == true) CustomerOrderPos.gameObject.SetActive(false);

        if (DataManager.Instance.NowOpen == true) StartCoroutine(SpawnDelay());
    }
}
