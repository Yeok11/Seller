using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVManager : MonoBehaviour
{
    public static CSVManager Instance;
    public CSVData csvdata = new CSVData();

    private void Awake()
    {
        Instance = this;

        csvdata.ItemData = CSVReader.Read("ItemData");
        csvdata.CustomMessage = CSVReader.Read("CustomerMEs");
        csvdata.DayEvent = CSVReader.Read("HappyDay");
        csvdata.EasyCustomMessage = CSVReader.Read("CustomerEasyMes");
        csvdata.achieve = CSVReader.Read("Achieve");

        if (DataManager.Instance.gameObject != null) DataManager.Instance.DataInput();

    }
}