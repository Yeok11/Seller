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
    }


}
