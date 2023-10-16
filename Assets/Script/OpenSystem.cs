using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class OpenSystem : MonoBehaviour, IPointerDownHandler
{
    DataManager DM;
    CustomerManager CM;
    TimeManager TM;

    TextMeshProUGUI Contants;

    private void Awake()
    {
        TM = GameObject.Find("Calendar").GetComponent<TimeManager>();
        DM = DataManager.Instance;
        CM = GameObject.Find("CustomerManager").GetComponent<CustomerManager>();
        Contants = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //OPEN일 때
        if(Contants.text == DM.OpCl[0])
        {
            Contants.text = DM.OpCl[2];
            DM.NowOpen = false;
            CM.ResetCustomer();
        }
        //CLOSE일 때
        else if (Contants.text == DM.OpCl[1])
        {
           Contants.text = DM.OpCl[0];
            DM.NowOpen = true;
            TM.TimeData = 0;
            CM.SpawnCustomer();
        }
        //다음 날일 때
        else
        {
            Contants.text = DM.OpCl[1];
        }
    }
}
