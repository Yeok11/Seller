using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SubSystemManager : SingleTon<SubSystemManager>
{
    GameObject BtList_Obj;

    [SerializeField] GameObject OrderItemType;
    [SerializeField] TextMeshProUGUI MarketSchedule;

    [SerializeField] private Transform BagContants;
    [SerializeField] Transform InteriorLists;

    [SerializeField] Transform MarketDataPos;
    [SerializeField] GameObject MarketItemInfo;

    public AudioMixer mixer;

    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider effectSlider;

    internal float PrimarySale;


    private void Start()
    {
        switch (DataManager.GameDif)
        {
            case Diff.Easy:
                PrimarySale = 0.15f;
                break;

            case Diff.Normal:
                PrimarySale = 0.5f;
                break;

            case Diff.Hard:
                PrimarySale = 0.3f;
                break;

            default:
                PrimarySale = 0f;
                break;
        }

        BtList_Obj = GameObject.Find("BtList");
        BtList_Obj.SetActive(false);

        BagItemReset();
        InteriorUpdate();
    }

    #region 소리
    public void MasterCtrl()
    {
        float volume = masterSlider.value;

        if (volume == -40f) mixer.SetFloat("Master", -80);
        else mixer.SetFloat("Master", volume);
    }

    public void BGMCtrl()
    {
        float volume = bgmSlider.value;

        if (volume == -40f) mixer.SetFloat("BGM", -80);
        else mixer.SetFloat("BGM", volume);
    }

    public void EffectCtrl()
    {
        float volume = effectSlider.value;

        if (volume == -40f) mixer.SetFloat("Effect", -80);
        else mixer.SetFloat("Effect", volume);
    }
    #endregion

    //전체 설정
    public void BtTurn(int BtType)
    {
        GameObject _BtList;

        if (BtType != 4) _BtList = BtList_Obj.transform.GetChild(BtType).GetChild(0).gameObject;
        else { _BtList = BtList_Obj; }

        if (_BtList.activeSelf == false) SubWindowOff(_BtList);
        else { _BtList.SetActive(false); }
    }
    public void SubWindowOff(GameObject OpenObject)
    {
        BtList_Obj.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        BtList_Obj.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        BtList_Obj.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
        BtList_Obj.transform.GetChild(3).GetChild(0).gameObject.SetActive(false);

        if (OpenObject != null) OpenObject.SetActive(true);
        if (OpenObject.name == "MarketWindow") MarketItemInfo.gameObject.SetActive(false);
        if (OpenObject.name == "BagWindow") BagItemReset();
    }

    //창고
    private void BagItemReset()
    {
        for (int i = 0; i < 15; i++)
        {
            BagContants.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.Instance.ItemType[i];
            BagContants.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "X " + DataManager.Instance.ItemCount[i];
            BagContants.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = DataManager.Instance.ItemPrice[i].ToString();
        }
    }

    //인테리어
    public void InteriorUpgrade(int j)
    {
        if (DataManager.Instance.NextCost[j, DataManager.Instance.InteriorLevel[j] -1] <= DataManager.Instance.HaveMoney && DataManager.Instance.InteriorLevel[j] < 5)
        {
            DataManager.Instance.HaveMoney -= DataManager.Instance.NextCost[j, DataManager.Instance.InteriorLevel[j] -1];
            DataManager.Instance.InteriorLevel[j]++;
            InteriorUpdate();
        }
        else
        {
            Debug.Log("잔액이 부족합니다.");
        }
    }
    private void InteriorUpdate()
    {
        for (int i = 0; i < 4; i++)
        {
            
            if (DataManager.Instance.InteriorLevel[i] < 5)
            {
                InteriorLists.GetChild(i).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Lv." + DataManager.Instance.InteriorLevel[i];
            }
            else
            {
                InteriorLists.GetChild(i).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Lv.Max";
            }
            InteriorLists.GetChild(i).GetChild(3).GetComponent<TextMeshProUGUI>().text = DataManager.Instance.NextCost[i, DataManager.Instance.InteriorLevel[i]-1].ToString();
        }

        InteriorLists.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = $"영업 시간 {DataManager.Instance.BonusPer[0,(DataManager.Instance.InteriorLevel[0] - 1)]}% 연장";
        InteriorLists.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = $"손님 방문속도 {DataManager.Instance.BonusPer[1, (DataManager.Instance.InteriorLevel[1] - 1)]}% 상승";
        InteriorLists.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().text = $"전 제품 매입가 {DataManager.Instance.BonusPer[2, (DataManager.Instance.InteriorLevel[2] - 1)]}% 감소";
        InteriorLists.GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>().text = $"전 제품 판매가 {DataManager.Instance.BonusPer[3, (DataManager.Instance.InteriorLevel[3] - 1)]}% 상승";
    }

    //상점
    private void MarketCntSign()
    {
        DataManager.Instance.MarketBtsPos.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = DataManager.Instance.MarketItemCnt.ToString();
    }
    internal void MarketScheduleUpdate()
    {
        MarketSchedule.text = $"다음 상인 방문까지 : {ScheduleJudge()}일";
    }
    private int ScheduleJudge()
    {
        if (DataManager.Instance.Days % 7 < 3)
        {
            return 3 - DataManager.Instance.Days % 7;
        }
        else
        {
            return 10 - DataManager.Instance.Days % 7;
        }
    }
    public void MarketCntUp(bool Ctrl)
    {
        if (Ctrl == true && DataManager.Instance.MarketItemCnt != 99) DataManager.Instance.MarketItemCnt += 1;
        else if (Ctrl == false && DataManager.Instance.MarketItemCnt != 0) DataManager.Instance.MarketItemCnt -= 1;
        
        MarketCntSign();
    }
    public void MarketBuyItem()
    {
        
        int j = 0;
        for (int i = 0; i < DataManager.Instance.ItemTypeCount; i++)
        {
            if (OrderItemType.GetComponent<TextMeshProUGUI>().text == DataManager.Instance.ItemType[i])
            {
                j = i;
                break;
            }
        }

        //매입가 = 판매가 - 인테리어 세일값 - 기본 20% 매입 감소
        int Sale = DataManager.Instance.ItemPrice[j] - (DataManager.Instance.ItemPrice[j] * DataManager.Instance.BonusPer[DataManager.Instance.InteriorLevel[2],2] / 100) - (int)(DataManager.Instance.ItemPrice[j] * PrimarySale);

        if (Sale * DataManager.Instance.MarketItemCnt <= DataManager.Instance.HaveMoney)
        {
            Debug.Log($"주문을 완료했습니다. -{Sale} X {DataManager.Instance.MarketItemCnt}");

            while (true)
            {
                DataManager.Instance.MarketOrderData.Add(j);
                DataManager.Instance.HaveMoney -= Sale;
                DataManager.Instance.MarketItemCnt--;

                if (DataManager.Instance.MarketItemCnt <= 0) break;
            }

            DataManager.Instance.UseGold[2] += Sale * DataManager.Instance.MarketItemCnt;

            MarketCntReset();
        }
        else
        {
            Debug.Log("보유한 돈이 부족합니다.");
        }
    }
    internal void MarketCntReset()
    {
        DataManager.Instance.MarketItemCnt = 0;
        MarketCntSign();
    }

    //설정
    public void Title()
    {
        SceneManager.LoadScene(1);
    }

    public void GameOff()
    {
        Application.Quit();
    }
}
