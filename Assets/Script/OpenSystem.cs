using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpenSystem : MonoBehaviour, IPointerDownHandler
{
    DataManager DM;
    TextMeshProUGUI Contants;

    internal static bool Check_WeekList;

    private void Awake()
    {
        DM = DataManager.Instance;

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
                if (Check_WeekList == false) TimeManager.Instance.WeekList.SetActive(true);

                Contants.text = DM.OpCl[2];
                DM.NowOpen = false;
                CustomerManager.Instance.OrderNowDo = false;
                CustomerManager.Instance.del = 0;
                StopAllCoroutines();
                CustomerManager.Instance.ResetCustomer(true);
                CustomerManager.Instance.CanSell = false;
                CounterManager.Instance.Monitor_Cancel();
                TimeManager.Instance.TimeData = 30;
                TimeManager.Instance.HourTimer();
            }
        }
        //���� �غ� -> ������
        else if (Contants.text == DM.OpCl[1])
        {
           Contants.text = DM.OpCl[0];
            DM.NowOpen = true;
            CustomerManager.Instance.OrderNowDo = false;
            SubSystemManager.Instance.CanUseBt = false;
            SubSystemManager.Instance.BtList_Obj.SetActive(false);
            StartCoroutine(CustomerManager.Instance.SpawnDelay());
        }
        //���� ���� -> ���� �غ�
        else if(Contants.text == DM.OpCl[2])
        {
            if (Check_WeekList == true)
            {
                Contants.text = DM.OpCl[1];
                StartCoroutine(NewdayEffect());
            }
            else
            {
                Debug.Log("�ְ������� Ȯ���ϼ���.");
            }
        }
    }

    IEnumerator NewdayEffect()
    {
        TimeManager.Instance.DayUpdateScene.gameObject.SetActive(true);
        TimeManager.Instance.DayUpdateScene.GetComponent<Image>().color = Color.clear;

        float a = 0;

        while (true)
        {
            a += 0.1f;
            TimeManager.Instance.DayUpdateScene.GetComponent<Image>().color = new Color(0, 0, 0, a);
            yield return new WaitForSeconds(0.1f);

            if (a >= 1) break;
        }

        TimeManager.Instance.NewDay();
    }
}
