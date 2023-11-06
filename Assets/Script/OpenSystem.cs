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

    internal static bool Check_WeekList;

    private void Awake()
    {
        DM = DataManager.Instance;
        CM = GameObject.Find("CustomerManager").GetComponent<CustomerManager>();
        Contants = GetComponentInChildren<TextMeshProUGUI>();

        Check_WeekList = true;

        Contants.text = DM.OpCl[1];
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //������ -> ���� ����
        if(Contants.text == DM.OpCl[0])
        {
            if (CustomerManager.Instance.OrderNowDo == true)
            {
                Debug.Log("�մ��� �ֹ� ������ ���� ���Ը� ���� �� �����ϴ�.");
            }
            else
            {
                if (Check_WeekList == false)
                {
                    TimeManager.Instance.WeekList.SetActive(true);
                }

                Contants.text = DM.OpCl[2];
                DM.NowOpen = false;
                CM.ResetCustomer();
                TimeManager.Instance.TimeData = 24;
                TimeManager.Instance.HourTimer();
            }
        }
        //���� �غ� -> ������
        else if (Contants.text == DM.OpCl[1])
        {
           Contants.text = DM.OpCl[0];
            DM.NowOpen = true;
            StartCoroutine(CM.SpawnDelay());
        }
        //���� ���� -> ���� �غ�
        else if(Contants.text == DM.OpCl[2])
        {
            if (Check_WeekList == true)
            {
                Contants.text = DM.OpCl[1];
                TimeManager.Instance.NewDay();
            }
            else
            {
                Debug.Log("�ְ������� Ȯ���ϼ���.");
            }
            
        }
    }
}
