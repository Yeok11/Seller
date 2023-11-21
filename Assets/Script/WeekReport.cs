using TMPro;
using UnityEngine;

public class WeekReport : MonoBehaviour
{
    [SerializeField] private Transform WeekData;
    [SerializeField] private Transform LastWeekData;
    [SerializeField] private Transform Report;

    DataManager DM;

    private void Start()
    {
        DM = DataManager.Instance;
    }

    public void OpenReport()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);

        InputWeekData();
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
        WeekData.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = (DM.BuyGold[2] - DM.UseGold[2]).ToString();
        WeekData.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = DM.ComeCustomerCnt[2].ToString();
        WeekData.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = DM.MissCnt[2].ToString();
        WeekData.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = DM.SellCnt[2].ToString();
        WeekData.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = DM.HaveMoney.ToString();

        LastWeekData.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = (DM.UseGold[2] - DM.UseGold[1]).ToString();
        LastWeekData.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = (DM.BuyGold[2] - DM.BuyGold[1]).ToString();
        LastWeekData.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().SetText((DM.ComeCustomerCnt[2] - DM.ComeCustomerCnt[1]).ToString());
        LastWeekData.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = (DM.MissCnt[2] - DM.MissCnt[1]).ToString();
        LastWeekData.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = (DM.SellCnt[2] - DM.SellCnt[1]).ToString();

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

        DM.UseGold.Add(1);
        DM.BuyGold.Add(1);
        DM.ComeCustomerCnt.Add(1);
        DM.MissCnt.Add(1);
    }

}
