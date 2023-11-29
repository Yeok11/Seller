using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CSVManager : MonoBehaviour
{
    public static CSVManager Instance;
    public CSVData csvdata = new CSVData();

    private void Awake()
    {
        Instance = this;

        csvdata.ItemData = CSVReader.Read("ItemData");
        csvdata.CustomMessage = CSVReader.Read("CustomerMEs");
        csvdata.EasyCustomMessage = CSVReader.Read("CustomerEasyMes");
        csvdata.BonusCustomMes = CSVReader.Read("CustomerMesBonus");
        csvdata.DayEvent = CSVReader.Read("HappyDay");
        csvdata.achieve = CSVReader.Read("Achieve");

        if (DataManager.Instance.gameObject != null && SceneManager.GetActiveScene().name != "Title") DataManager.Instance.DataInput();
    }
}