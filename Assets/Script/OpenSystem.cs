using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class OpenSystem : MonoBehaviour, IPointerDownHandler
{
    DataManager DM;
    CustomerManager CM;

    TextMeshProUGUI Contants;

    private void Awake()
    {
        DM = DataManager.Instance;
        CM = GameObject.Find("CustomerManager").GetComponent<CustomerManager>();
        Contants = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //OPEN -> 다음 날
        if(Contants.text == DM.OpCl[0])
        {
            if (CustomerManager.Instance.OrderNowDo == true)
            {
                Debug.Log("손님이 주문 도중일 때는 가게를 닫을 수 없습니다.");
            }
            else
            {
                Contants.text = DM.OpCl[2];
                DM.NowOpen = false;
                CM.ResetCustomer();
                TimeManager.Instance.TimeData = 24;
                TimeManager.Instance.HourTimer();
            }
        }
        //CLOSE -> OPEN
        else if (Contants.text == DM.OpCl[1])
        {
           Contants.text = DM.OpCl[0];
            DM.NowOpen = true;
            CM.SpawnCustomer();
        }
        //다음 날 -> OPEN
        else if(Contants.text == DM.OpCl[2])
        {
            Contants.text = DM.OpCl[1];
            TimeManager.Instance.NewDay();
        }
    }
}
