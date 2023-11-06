using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeekReport : MonoBehaviour
{
    public void OpenReport()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void Sign()
    {
        transform.GetChild(1).gameObject.SetActive(false);
        OpenSystem.Check_WeekList = true;
    }
}
