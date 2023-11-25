using System;
using TMPro;
using UnityEngine;

public class WeekReport : MonoBehaviour
{
    [SerializeField] private Transform WeekData;
    [SerializeField] private Transform LastWeekData;
    [SerializeField] private Transform Report;

    DataManager DM;

    Func<int, int, string> CompareResult;

    public Action _SignResult;

    private void Start()
    {
        CompareResult = (int Now, int Before) => { int sum = Now - Before; return DataManager.Sign_PlusMinus(sum).ToString(); };

        DM = DataManager.Instance;
    }

    public void OpenReport()
    {
        if (DataManager.GameDif != Diff.Event_1) transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);

        if (DataManager.GameDif != Diff.Event_1)
        {
            InputWeekData();
        }
    }

    public void Sign()
    {
        
        transform.GetChild(1).gameObject.SetActive(false);
        OpenSystem.Check_WeekList = true;
    }

    private void InputWeekData()
    {
        WeekData.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = DM.UseGold[2].ToString();
        WeekData.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = DM.BuyGold[2].ToString();
        WeekData.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = CompareResult(DM.BuyGold[0], DM.UseGold[0]);
        WeekData.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = DM.ComeCustomerCnt[2].ToString();
        WeekData.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = DM.MissCnt[2].ToString();
        WeekData.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = DM.SellCnt[2].ToString();
        WeekData.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = DM.HaveMoney.ToString();

        LastWeekData.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = CompareResult(DM.UseGold[2], DM.UseGold[1]);
        LastWeekData.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = CompareResult(DM.BuyGold[2], DM.BuyGold[1]);
        LastWeekData.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().SetText(CompareResult(DM.ComeCustomerCnt[2], DM.ComeCustomerCnt[1]));
        LastWeekData.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = CompareResult(DM.MissCnt[2], DM.MissCnt[1]);
        LastWeekData.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = CompareResult(DM.SellCnt[2], DM.SellCnt[1]);

        DataUpdate();
    }




    private void DataUpdate()
    {
        DM.UseGold[0] += DM.UseGold[2];
        DM.BuyGold[0] += DM.BuyGold[2];
        DM.ComeCustomerCnt[0] += DM.ComeCustomerCnt[2];
        DM.MissCnt[0] += DM.MissCnt[2];
        DM.SellCnt[0] += DM.SellCnt[2];

        DM.UseGold.RemoveAt(1);
        DM.BuyGold.RemoveAt(1);
        DM.ComeCustomerCnt.RemoveAt(1);
        DM.MissCnt.RemoveAt(1);
        DM.SellCnt.RemoveAt(1);

        DM.UseGold.Add(1);
        DM.BuyGold.Add(1);
        DM.ComeCustomerCnt.Add(1);
        DM.MissCnt.Add(1);
        DM.SellCnt.Add(1);
    }

}
