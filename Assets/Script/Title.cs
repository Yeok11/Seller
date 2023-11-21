using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Title : MonoBehaviour
{
    [SerializeField] private GameObject TitleMes;
    [SerializeField] private GameObject Set;

    [SerializeField] private GameObject ChooseStage;
    [SerializeField] private GameObject Achieve;
    [SerializeField] private GameObject Option;

    [SerializeField] private GameObject ChooseMod;
    [SerializeField] private GameObject DiffMod;
    [SerializeField] private GameObject EventMod;

    [SerializeField] private GameObject BlackEffect;

    [SerializeField] private Transform DataPos;

    int[] PlayCnt = { 0, 0, 0};
    int[] BestScore = { 0,0,0 };
    int[] BestGetMoney = { 0,0,0 };

    int BestHavingMoney;
    float AllSatisfy;
    int AllCustomerCnt;
    int AllMissCnt;
    int BestMissCnt;
    int AllBuyItemCnt;
    int BestBuyItemCnt;
    int AllSellItemCnt;
    int BestSellItemCnt;

    bool SetOn = false;

    private void Awake()
    {
        SizeCtrl();
    }

    private void Start()
    {
        //DataSave();
        DataLoad();
    }

    private void DataSave()
    {
        string strArr = "";
        string strArr1 = "";
        string strArr2 = "";

        for (int i = 0; i < PlayCnt.Length; i++)
        {
            strArr = strArr + PlayCnt[i];
            strArr1 = strArr1 + BestScore[i];
            strArr2 = strArr2 + BestGetMoney[i];

            if (i < PlayCnt.Length - 1)
            {
                strArr = strArr + ",";
                strArr1 = strArr1 + ",";
                strArr2 = strArr2 + ",";
            }
        }

        PlayerPrefs.SetString("PlayCntData", strArr);
        PlayerPrefs.SetString("ScoreData", strArr1);
        PlayerPrefs.SetString("GetMoneyData", strArr2);

        PlayerPrefs.SetInt("HaveMoney", BestHavingMoney);
        PlayerPrefs.SetFloat("Satisfy", AllSatisfy);
        PlayerPrefs.SetInt("Customer", AllCustomerCnt);
        PlayerPrefs.SetInt("AllMiss", AllMissCnt);
        PlayerPrefs.SetInt("Miss", BestMissCnt);
        PlayerPrefs.SetInt("AllBuy", AllBuyItemCnt);
        PlayerPrefs.SetInt("Buy", BestBuyItemCnt);
        PlayerPrefs.SetInt("AllSell", AllSellItemCnt);
        PlayerPrefs.SetInt("Sell", BestSellItemCnt);
    }

    public void DataReset()
    {
        for (int i = 0; i < PlayCnt.Length; i++)
        {
            PlayCnt[i] = 0;
            BestScore[i] = 0;
            BestGetMoney[i] = 0;
        }
        DataSave();
    }

    private void DataLoad()
    {
        string[] dataArr = PlayerPrefs.GetString("PlayCntData").Split(',');
        string[] dataArr1 = PlayerPrefs.GetString("ScoreData").Split(',');
        string[] dataArr2 = PlayerPrefs.GetString("GetMoneyData").Split(',');

        for (int i = 0; i < PlayCnt.Length; i++)
        {
            PlayCnt[i] = System.Convert.ToInt32(dataArr[i]);
            BestScore[i] = System.Convert.ToInt32(dataArr1[i]);
            BestGetMoney[i] = System.Convert.ToInt32(dataArr2[i]);
        }

        BestHavingMoney = PlayerPrefs.GetInt("HaveMoney");
        AllSatisfy = PlayerPrefs.GetFloat("Satisfy");
        AllMissCnt = PlayerPrefs.GetInt("AllMiss");
        BestMissCnt = PlayerPrefs.GetInt("Miss");
        AllBuyItemCnt = PlayerPrefs.GetInt("AllBuy");
        BestBuyItemCnt = PlayerPrefs.GetInt("Buy");
        AllBuyItemCnt = PlayerPrefs.GetInt("AllBuy");
        BestBuyItemCnt = PlayerPrefs.GetInt("Buy");
        AllSellItemCnt = PlayerPrefs.GetInt("AllSell");
        BestSellItemCnt = PlayerPrefs.GetInt("Sell");
        



        DataPos.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("�� ��� Ƚ�� : " + (PlayCnt[0]+PlayCnt[1]+PlayCnt[2]));
        DataPos.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("���� ��� �� : " + PlayCnt[0]);
        DataPos.GetChild(2).GetComponent<TextMeshProUGUI>().SetText("���� �ְ� ���� : " + BestScore[0]);
        DataPos.GetChild(3).GetComponent<TextMeshProUGUI>().SetText("���� ��� �� : " + PlayCnt[1]);
        DataPos.GetChild(4).GetComponent<TextMeshProUGUI>().SetText("���� �ְ� ���� : " + BestScore[1]);
        DataPos.GetChild(5).GetComponent<TextMeshProUGUI>().SetText("����� ��� �� : " + PlayCnt[2]);
        DataPos.GetChild(6).GetComponent<TextMeshProUGUI>().SetText("����� �ְ� ���� : " + BestScore[2]);
        DataPos.GetChild(8).GetComponent<TextMeshProUGUI>().SetText("�� ����� : " + (BestGetMoney[0] + BestGetMoney[1] + BestGetMoney[2]));
        DataPos.GetChild(9).GetComponent<TextMeshProUGUI>().SetText("���� �ִ� ����� : " + BestGetMoney[0]);
        DataPos.GetChild(10).GetComponent<TextMeshProUGUI>().SetText("���� �ִ� ����� : " + BestGetMoney[1]);
        DataPos.GetChild(11).GetComponent<TextMeshProUGUI>().SetText("����� �ִ� ����� : " + BestGetMoney[2]);
        DataPos.GetChild(12).GetComponent<TextMeshProUGUI>().SetText("�ִ� ������ : " + BestHavingMoney);
        DataPos.GetChild(14).GetComponent<TextMeshProUGUI>().SetText("�� ������ : " + AllSatisfy + "%");
        DataPos.GetChild(14).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("�� �մ� �� : " + AllCustomerCnt);
        DataPos.GetChild(15).GetComponent<TextMeshProUGUI>().SetText("�� �Ǽ� Ƚ�� : " + AllMissCnt);
        DataPos.GetChild(15).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("�ִ� �Ǽ� Ƚ�� : " + BestMissCnt);
        DataPos.GetChild(16).GetComponent<TextMeshProUGUI>().SetText("�� ���� Ƚ�� : " + AllBuyItemCnt);
        DataPos.GetChild(16).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("�ִ� ���� �� : " + BestBuyItemCnt);
        DataPos.GetChild(17).GetComponent<TextMeshProUGUI>().SetText("�� �Ǹ� �� : " + AllSellItemCnt);
        DataPos.GetChild(17).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("�ִ� �Ǹ� �� : " + BestSellItemCnt);
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) && TitleMes.activeSelf == false)
        {
            if (Option.activeSelf == true) Option.SetActive(false);
            else if (Achieve.activeSelf == true) Achieve.SetActive(false);
            else
            {
                if (SetOn == true)
                {
                    SetOn = false;
                    if (ChooseMod.activeSelf == true)
                    {
                        ChooseStage.SetActive(false);
                        ChooseSet();
                    }
                    else
                    {
                        DiffMod.SetActive(false);
                        EventMod.SetActive(false);
                        StageSet();
                    }
                }
                else
                {
                    Set.SetActive(false);
                    BlackEffect.SetActive(false);
                    TitleMes.SetActive(true);
                }
            }
        }
    }

    public void AchieveSet()
    {
        Achieve.SetActive(true);
        TitleMes.SetActive(false);
    }

    public void OptionSet()
    {
        Option.SetActive(true);
        TitleMes.SetActive(false);
        BlackEffect.SetActive(true);
    }

    public void StageSet()
    {
        ChooseMod.SetActive(true);
        ChooseStage.SetActive(true);
        Set.SetActive(false);
        BlackEffect.SetActive(false);
        SetOn = true;
    }

    public void DiffSet()
    {
        ChooseMod.SetActive(false);
        DiffMod.SetActive(true);
    }

    public void EventSet()
    {
        ChooseMod.SetActive(false);
        EventMod.SetActive(true);
    }

    public void ChooseSet()
    {
        Set.SetActive(true);
        BlackEffect.SetActive(true);
        TitleMes.SetActive(false);
    }

    public void InGameStart(string _dif)
    {
        switch (_dif)
        {
            case "Easy":
                DataManager.GameDif = Diff.Easy;
                break;
            case "Normal":
                DataManager.GameDif = Diff.Normal;
                break;
            case "Hard":
                DataManager.GameDif = Diff.Hard;
                break;
            case "Event1":
                DataManager.GameDif = Diff.Event_1;
                break;
        }
        
        SceneManager.LoadScene(0);
    }

    static public void SizeCtrl()
    {
        int setWidth = 1920; // ����� ���� �ʺ�
        int setHeight = 1080; // ����� ���� ����

        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����
        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // ���ο� ����
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
        }
    }
}
