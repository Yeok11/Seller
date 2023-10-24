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
        //OPEN -> ���� ��
        if(Contants.text == DM.OpCl[0])
        {
            if (CustomerManager.Instance.OrderNowDo == true)
            {
                Debug.Log("�մ��� �ֹ� ������ ���� ���Ը� ���� �� �����ϴ�.");
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
        //���� �� -> OPEN
        else if(Contants.text == DM.OpCl[2])
        {
            Contants.text = DM.OpCl[1];
            TimeManager.Instance.NewDay();
        }
    }
}
