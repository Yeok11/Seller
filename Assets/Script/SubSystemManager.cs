using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSystemManager : MonoBehaviour
{
    GameObject BtList;

    private void Awake()
    {
        BtList = GameObject.Find("BtList");
        BtList.SetActive(false);
    }

    public void BtTurn(int BtType)
    {
        GameObject _BtList;

        if (BtType != 4)
        {
            _BtList = BtList.transform.GetChild(BtType).GetChild(0).gameObject;
        }
        else
        {
            _BtList = BtList;
        }

        if (_BtList.activeSelf == false) _BtList.SetActive(true);
        else { _BtList.SetActive(false); }
    }

    /*
    void SubWindowOff(GameObject OpenObject)
    {
        SetActive(false);
        Receipt_window.SetActive(false);
        Option_window.SetActive(false);
        Upgrade_window.SetActive(false);

        if (OpenObject != null) OpenObject.SetActive(true);
    }*/
}
