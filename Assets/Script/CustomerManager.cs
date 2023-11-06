using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomerManager : SingleTon<CustomerManager>
{
    GameObject NowCustomer;
    [SerializeField] Transform CustomerOrderPos;
    internal bool OrderNowDo;

    int CustomerType;

    IEnumerator CustomerOrder()
    {
        yield return new WaitForSeconds(0.25f);

        CustomerOrderPos.GetChild(0).gameObject.SetActive(true);

        yield return new WaitForSeconds(0.15f);

        CustomerOrderPos.GetChild(1).gameObject.SetActive(true);
        CustomerOrderPos.GetChild(1).GetComponent<TextMeshProUGUI>().text = DataManager.Instance.Message[CustomerType, FindRange()];

        OrderNowDo = false;
    }

    int FindRange()
    {
        int i;
        for (i = 0; i < DataManager.Instance.Message.GetLength(0); i++)
        {
            if (DataManager.Instance.Message[CustomerType, i] == null)
            {
                break;
            }
        }
        back:

        int j = Random.Range(0, i);

        if (DataManager.Instance.Message[CustomerType,j] == "null") goto back;

        return j;
    }

    internal IEnumerator SpawnDelay()
    {
        float del = Random.Range(1f - DataManager.Instance.BonusPer[1, DataManager.Instance.InteriorLevel[1]] /
            (100 + DataManager.Instance.BonusPer[1, DataManager.Instance.InteriorLevel[1]]),

            4f - DataManager.Instance.BonusPer[1,DataManager.Instance.InteriorLevel[1]] /
            (100 + DataManager.Instance.BonusPer[1, DataManager.Instance.InteriorLevel[1]]));

        Debug.Log(del);
        OrderNowDo = true;

        yield return new WaitForSeconds(del);

        if (DataManager.Instance.NowOpen == true)
        {
            SpawnCustomer();
        }
    }

    internal void SpawnCustomer()
    {
        NowCustomer = transform.GetChild(RandomCustomer()).gameObject;
        NowCustomer.SetActive(true);
        NowCustomer.transform.GetChild(Random.Range(0, 2)).gameObject.SetActive(true);

        StartCoroutine(CustomerOrder());
    }

    internal void ResetCustomer()
    {
        if (NowCustomer != null)
        {
            NowCustomer.SetActive(false);
        }
        CustomerOrderPos.GetChild(0).gameObject.SetActive(false);
        CustomerOrderPos.GetChild(1).gameObject.SetActive(false);

        if (DataManager.Instance.NowOpen == true) StartCoroutine(SpawnDelay());
    }

    private int RandomCustomer()
    {
        CustomerType = Random.Range(1, 14);

        //µµ±¼²Û
        if (DataManager.Instance.Days >= 7) CustomerType = Random.Range(1, 18);
        //¼ºÁ÷ÀÚ
        if (DataManager.Instance.Days >= 15) CustomerType = Random.Range(1, 20);
        //±ÍÁ·
        if (DataManager.Instance.Days >= 40) CustomerType = Random.Range(1, 21);

        switch (CustomerType)
        {
            case <=8:
                return CustomerType = 0;

            case <=11:
                return CustomerType = 1;

            case <=14:
                return CustomerType = 2;

            case <=17:
                return CustomerType = 3;

            case <=19:
                return CustomerType = 4;

            case <=20:
                return CustomerType = 5;


            //¿¹¿Ü
            default:
                return 100;
        }
    }
}
