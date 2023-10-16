using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpenSystem : MonoBehaviour
{
    DataManager DM;
    CustomerManager CM;
    TimeManager TM;

    private void Awake()
    {
        TM = GameObject.Find("Calendar").GetComponent<TimeManager>();
        DM = GameObject.Find("GameManager").GetComponent<DataManager>();
        CM = GameObject.Find("CustomerManager").GetComponent<CustomerManager>();
    }

    public void ChangeSign(int NextDo)
    {
        switch (NextDo)
        {
            case 1:
                GetComponentInChildren<TextMeshProUGUI>().text = DM.OpCl[0];
                DM.NowOpen = true;
                TM.TimeData = 0;
                CM.SpawnCustomer();
                break;

            case 2:
                GetComponentInChildren<TextMeshProUGUI>().text = DM.OpCl[2];
                DM.NowOpen = false;
                CM.ResetCustomer();
                break;

            case 3:
                break;

            default:
                break;
        }
    }
}
