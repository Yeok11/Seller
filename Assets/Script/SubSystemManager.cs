using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSystemManager : MonoBehaviour
{
    GameObject BtList_Obj;

    private void Awake()
    {
        BtList_Obj = GameObject.Find("BtList");
        BtList_Obj.SetActive(false);
    }

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
    }
}
