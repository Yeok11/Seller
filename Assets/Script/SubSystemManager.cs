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

    #region �Ҹ�
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

    //��ü ����
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

    //â��
    private void BagItemReset()
    {
        for (int i = 0; i < 15; i++)
        {
            BagContants.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = DataManager.Instance.ItemType[i];
            BagContants.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "X " + DataManager.Instance.ItemCount[i];
            BagContants.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().text = DataManager.Instance.ItemPrice[i].ToString();
        }
    }

    //���׸���
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
            Debug.Log("�ܾ��� �����մϴ�.");
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

        InteriorLists.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = $"���� �ð� {DataManager.Instance.BonusPer[0,(DataManager.Instance.InteriorLevel[0] - 1)]}% ����";
        InteriorLists.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>().text = $"�մ� �湮�ӵ� {DataManager.Instance.BonusPer[1, (DataManager.Instance.InteriorLevel[1] - 1)]}% ���";
        InteriorLists.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().text = $"�� ��ǰ ���԰� {DataManager.Instance.BonusPer[2, (DataManager.Instance.InteriorLevel[2] - 1)]}% ����";
        InteriorLists.GetChild(3).GetChild(2).GetComponent<TextMeshProUGUI>().text = $"�� ��ǰ �ǸŰ� {DataManager.Instance.BonusPer[3, (DataManager.Instance.InteriorLevel[3] - 1)]}% ���";
    }

    //����
    private void MarketCntSign()
    {
        DataManager.Instance.MarketBtsPos.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = DataManager.Instance.MarketItemCnt.ToString();
    }
    internal void MarketScheduleUpdate()
    {
        MarketSchedule.text = $"���� ���� �湮���� : {ScheduleJudge()}��";
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

        //���԰� = �ǸŰ� - ���׸��� ���ϰ� - �⺻ 20% ���� ����
        int Sale = DataManager.Instance.ItemPrice[j] - (DataManager.Instance.ItemPrice[j] * DataManager.Instance.BonusPer[DataManager.Instance.InteriorLevel[2],2] / 100) - (int)(DataManager.Instance.ItemPrice[j] * PrimarySale);

        if (Sale * DataManager.Instance.MarketItemCnt <= DataManager.Instance.HaveMoney)
        {
            Debug.Log($"�ֹ��� �Ϸ��߽��ϴ�. -{Sale} X {DataManager.Instance.MarketItemCnt}");

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
            Debug.Log("������ ���� �����մϴ�.");
        }
    }
    internal void MarketCntReset()
    {
        DataManager.Instance.MarketItemCnt = 0;
        MarketCntSign();
    }

    //����
    public void Title()
    {
        SceneManager.LoadScene(1);
    }

    public void GameOff()
    {
        Application.Quit();
    }
}
