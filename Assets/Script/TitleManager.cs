using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using EasyJson;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject TitleMes;

    [SerializeField] private GameObject Set;
    [SerializeField] private GameObject ChooseStage;
    [SerializeField] private GameObject Achieve;
    [SerializeField] private GameObject Option;

    private GameObject ChooseMod;
    private GameObject DiffMod;
    private GameObject EventMod;

    [SerializeField] private Transform AchieveContants;

    [SerializeField] private GameObject BlackEffect;
    [SerializeField] private GameObject ContinueCheck;

    private Transform OptionData;

    [SerializeField] private TextMeshProUGUI StageContants;
    [SerializeField] private TextMeshProUGUI EventContants;
    [SerializeField] private GameObject DiffSelector;

    static public bool ContinueData;


    int[] PlayCnt = { 0, 0, 0};
    static internal int[] BestScore = { 0,0,0 };
    static internal int[] BestGetMoney = { 0,0,0 };

    internal List<string> achieveType = new List<string>();

    bool SetOn = false;

    private void Awake()
    {
        SizeCtrl();
        if (EasyToJson.FromJson<GamePlayData>("RecordPlay").M_Day != 0) ContinueData = true;

        Transform Mod = ChooseStage.transform.GetChild(0);
        OptionData = Option.transform.GetChild(0);
        ChooseMod = Mod.GetChild(0).gameObject;
        DiffMod = Mod.GetChild(1).gameObject;
        EventMod = Mod.GetChild(2).gameObject;
    }

    private void Start()
    {
        for (int i = 0; i < CSVManager.Instance.csvdata.achieve.Count; i++)
        {
            achieveType.Add(CSVManager.Instance.csvdata.achieve[i]["이름"].ToString());
            //Debug.Log(achieveType[i]);
        }

        

        DataLoad();
        

        AchieveDataInput();
    }

    void AchieveDataInput()
    {
        for (int i = 0; i < AchieveContants.GetChild(0).childCount; i++)
        {
            try
            {
                AchieveContants.GetChild(0).GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().SetText(achieveType[i] + "");
            }
            catch
            {
                break;
            }
        }
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

        if (PlayerPrefs.GetInt("Customer") == 0) PlayerPrefs.SetFloat("Satisfy", 100);
        else PlayerPrefs.SetFloat("Satisfy", 100 - PlayerPrefs.GetInt("AllMiss") / PlayerPrefs.GetInt("Customer") * 100);

        DataLoad();
    }

    public void DataReset()
    {
        for (int i = 0; i < PlayCnt.Length; i++)
        {
            PlayCnt[i] = 0;
            BestScore[i] = 0;
            BestGetMoney[i] = 0;
        }

        PlayerPrefs.SetInt("HaveMoney", 0);
        PlayerPrefs.SetFloat("Satisfy", 0);
        PlayerPrefs.SetInt("Customer", 0);
        PlayerPrefs.SetInt("AllMiss", 0);
        PlayerPrefs.SetInt("Miss", 0);
        PlayerPrefs.SetInt("AllBuy", 0);
        PlayerPrefs.SetInt("Buy", 0);
        PlayerPrefs.SetInt("AllSell", 0);
        PlayerPrefs.SetInt("Sell", 0);

        DataSave();
    }

    private void DataLoad()
    {
        string[] dataArr = PlayerPrefs.GetString("PlayCntData").Split(',');
        string[] dataArr1 = PlayerPrefs.GetString("ScoreData").Split(',');
        string[] dataArr2 = PlayerPrefs.GetString("GetMoneyData").Split(',');

        for (int i = 0; i < PlayCnt.Length; i++)
        {
            try
            {
                PlayCnt[i] = System.Convert.ToInt32(dataArr[i]);
                BestScore[i] = System.Convert.ToInt32(dataArr1[i]);
                BestGetMoney[i] = System.Convert.ToInt32(dataArr2[i]);
            }
            catch
            {
                PlayCnt[i] = 0;
                BestScore[i] = 0;
                BestGetMoney[i] = 0;
            }
        }


        OptionData.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("총 장사 횟수 : " + (PlayCnt[0]+PlayCnt[1]+PlayCnt[2]));
        OptionData.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("장사꾼 장사 수 : " + PlayCnt[0]);
        OptionData.GetChild(2).GetComponent<TextMeshProUGUI>().SetText("장사꾼 최고 점수 : " + BestScore[0]);
        OptionData.GetChild(3).GetComponent<TextMeshProUGUI>().SetText("상인 장사 수 : " + PlayCnt[1]);
        OptionData.GetChild(4).GetComponent<TextMeshProUGUI>().SetText("상인 최고 점수 : " + BestScore[1]);
        OptionData.GetChild(5).GetComponent<TextMeshProUGUI>().SetText("대상인 장사 수 : " + PlayCnt[2]);
        OptionData.GetChild(6).GetComponent<TextMeshProUGUI>().SetText("대상인 최고 점수 : " + BestScore[2]);
        OptionData.GetChild(8).GetComponent<TextMeshProUGUI>().SetText("총 매출액 : " + (BestGetMoney[0] + BestGetMoney[1] + BestGetMoney[2]));
        OptionData.GetChild(9).GetComponent<TextMeshProUGUI>().SetText("장사꾼 최대 매출액 : " + BestGetMoney[0]);
        OptionData.GetChild(10).GetComponent<TextMeshProUGUI>().SetText("상인 최대 매출액 : " + BestGetMoney[1]);
        OptionData.GetChild(11).GetComponent<TextMeshProUGUI>().SetText("대상인 최대 매출액 : " + BestGetMoney[2]);
        OptionData.GetChild(12).GetComponent<TextMeshProUGUI>().SetText("최대 보유금 : " + PlayerPrefs.GetInt("HaveMoney"));
        OptionData.GetChild(14).GetComponent<TextMeshProUGUI>().SetText("총 만족도 : " + PlayerPrefs.GetFloat("Satisfy") + "%");
        OptionData.GetChild(14).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("총 손님 수 : " + PlayerPrefs.GetInt("Customer"));
        OptionData.GetChild(15).GetComponent<TextMeshProUGUI>().SetText("총 실수 횟수 : " + PlayerPrefs.GetInt("AllMiss"));
        OptionData.GetChild(15).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("최대 실수 횟수 : " + PlayerPrefs.GetInt("Miss"));
        OptionData.GetChild(16).GetComponent<TextMeshProUGUI>().SetText("총 매입 횟수 : " + PlayerPrefs.GetInt("AllBuy"));
        OptionData.GetChild(16).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("최대 매입 수 : " + PlayerPrefs.GetInt("Buy"));
        OptionData.GetChild(17).GetComponent<TextMeshProUGUI>().SetText("총 판매 수 : " + PlayerPrefs.GetInt("AllSell"));
        OptionData.GetChild(17).GetChild(0).GetComponent<TextMeshProUGUI>().SetText("최대 판매 수 : " + PlayerPrefs.GetInt("Sell"));
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) && TitleMes.activeSelf == false)
        {
            if (ContinueCheck.activeSelf)
            {
                ContinueCheck.SetActive(false);
            }
            else
            {
                if (Option.activeSelf == true)
                {
                    Option.SetActive(false);
                    GetComponent<AudioSource>().Play();
                }
                else if (Achieve.activeSelf == true)
                {
                    Achieve.SetActive(false);
                    GetComponent<AudioSource>().Play();
                }
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
                            DiffSelector.SetActive(false);
                            StageContants.SetText("난이도를 선택하세요.");
                            EventContants.SetText("모드를 선택하세요.");
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
                        GetComponent<AudioSource>().Play();
                    }
                }
            }
        }
    }

    public void ContinueGame()
    {
        if (DataManager.GameDif== Diff.Event_1) SceneManager.LoadScene("Ingame Event");
        else
        {
            SceneManager.LoadScene("Ingame");
        }
        
    }
    public void NewGame()
    {
        SubSystemManager.Instance.DataReset();
        ContinueCheck.SetActive(false);
        StageSet();
    }

    public void AchieveSet()
    {
        Achieve.SetActive(true);
        Achieve.transform.GetChild(0).GetComponentInChildren<Scrollbar>().value = 1;
        TitleMes.SetActive(false);
        GetComponent<AudioSource>().Play();
    }

    public void OptionSet()
    {
        Option.SetActive(true);
        TitleMes.SetActive(false);
        BlackEffect.SetActive(true);
        GetComponent<AudioSource>().Play();
    }

    public void StageSet()
    {
        int days = 0;

        
        try
        {
            days = EasyToJson.FromJson<GamePlayData>("RecordPlay").M_Day;
            DataManager.GameDif = EasyToJson.FromJson<GamePlayData>("RecordPlay").diff;
        }
        catch
        {
            days = 0;
        }
        
        

        if (ContinueData)
        {
            ContinueCheck.SetActive(true);
            ContinueCheck.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText($"기존 데이터가 존재합니다. 이어하시겠습니까?\n" +
                $"난이도 -{DataManager.GameDif}- / {days}일차");
        }
        else
        {
            ChooseMod.SetActive(true);
            ChooseStage.SetActive(true);
            Set.SetActive(false);
            BlackEffect.SetActive(false);
            SetOn = true;
        }
        GetComponent<AudioSource>().Play();
    }

    public void DiffSet()
    {
        ChooseMod.SetActive(false);
        DiffMod.SetActive(true);
        GetComponent<AudioSource>().Play();
    }

    public void EventSet()
    {
        ChooseMod.SetActive(false);
        EventMod.SetActive(true);
        GetComponent<AudioSource>().Play();
    }

    public void ChooseSet()
    {
        Set.SetActive(true);
        BlackEffect.SetActive(true);
        TitleMes.SetActive(false);
        GetComponent<AudioSource>().Play();
    }

    public void DiffChoose()
    {
        if ((int)DataManager.GameDif < 3) PlayCnt[(int)DataManager.GameDif]++;
        DataSave();
        if (DataManager.GameDif == Diff.Event_1) SceneManager.LoadScene("Ingame Event");
        else if (DataManager.GameDif == Diff.Tutorial) SceneManager.LoadScene("Tutorial");
        else
        {
            SceneManager.LoadScene("Ingame");
        }
    }

    public void InGameStart(string _dif)
    {
        GetComponent<AudioSource>().Play();
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
            case "Tutorial":
                DataManager.GameDif = Diff.Tutorial;
                DiffChoose();
                return;

            case "Event2":
                DataManager.GameDif = Diff.Event_2;
                break;

            case "Event3":
                DataManager.GameDif = Diff.Event_3;
                break;
        }

        if (DiffSelector.activeSelf == false) DiffSelector.SetActive(true);


        if (DiffMod.activeSelf == true)
        {
            DiffSelector.transform.SetParent(DiffMod.transform.GetChild((int)DataManager.GameDif + 1));
            StageContants.SetText("근무 일 : " + ((int)DataManager.GameDif < 3 ? 50 : 30) + "\n아이템 갯수 : 존재" + "\n난이도 : " + DataManager.GameDif + "\n매입 감소가 : " + SubSystemManager.Instance.SalePer() + "%");
        }
        else if (EventMod.activeSelf)
        {
            if (DataManager.GameDif != Diff.Event_1)
            {
                EventContants.SetText("아직 모드가 완성되지 않았습니다. 다른 모드를 선택해주세요.");
                DiffSelector.SetActive(false);
                return;
            }
            DiffSelector.transform.SetParent(EventMod.transform.GetChild((int)DataManager.GameDif % 10 + 1));
            EventContants.SetText("근무 일 : " + ((int)DataManager.GameDif < 3 ? 50 : 30) + "\n아이템 갯수 : 없음" + "\n난이도 : " + (DataManager.GameDif == Diff.Event_1 ? "넘치는 물품" : "null") + "\n매입 감소가 : " + SubSystemManager.Instance.SalePer() + "%");
        }

        DiffSelector.transform.position = DiffSelector.transform.parent.position;

        
    }

    static public void SizeCtrl()
    {
        int setWidth = 1920; // 사용자 설정 너비
        int setHeight = 1080; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장
        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }

    public void GameOff()
    {
        Application.Quit();
    }
}
