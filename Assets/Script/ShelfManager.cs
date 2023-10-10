using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[HideInInspector]
public class ShelfManager : MonoBehaviour
{
    private GameObject Shelf;
    private GameObject[] Slot = new GameObject[15];

    DataManager DM;

    void Start()
    {
        DM = GetComponent<DataManager>();

        Shelf = GameObject.Find("Shelf_Contants");

        for (int i = 0; i < Shelf.transform.childCount; i++)
        {
            Slot[i] = Shelf.transform.GetChild(i).gameObject;

            Slot[i].transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = DM.ItemCount[i].ToString();
            Slot[i].transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = DM.ItemPrice[i].ToString();
        }

        
    }

    
}
