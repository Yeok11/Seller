using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : SingleTon<CustomerManager>
{
    GameObject NowCustomer;
    [SerializeField] Transform CustomerOrderPos;
    internal bool OrderNowDo;

    IEnumerator CustomerOrder()
    {
        OrderNowDo = true;
        
        yield return new WaitForSeconds(0.25f);

        CustomerOrderPos.GetChild(0).gameObject.SetActive(true);

        yield return new WaitForSeconds(0.15f);

        CustomerOrderPos.GetChild(1).gameObject.SetActive(true);

        OrderNowDo = false;
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
        NowCustomer.SetActive(false);
        CustomerOrderPos.GetChild(0).gameObject.SetActive(false);
        CustomerOrderPos.GetChild(1).gameObject.SetActive(false);

        if (DataManager.Instance.NowOpen == true) SpawnCustomer();
    }

    private int RandomCustomer()
    {
        int i = Random.Range(1, 21);
        switch (i)
        {
            case <=8:
                return 0;

            case <=11:
                return 1;

            case <=14:
                return 2;

            case <=17:
                return 3;

            case <=19:
                return 4;

            case <=20:
                return 5;

            default:
                return 100;
        }
    }
}
