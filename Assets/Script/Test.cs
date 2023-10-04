using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    string[] ItemType = new string[15];

    private void Start()
    {
        
    }

    public void A()
    {
        Debug.Log(CSVManager.Instance.csvdata.ItemData[1]["ItemName"].ToString());
    }
}
