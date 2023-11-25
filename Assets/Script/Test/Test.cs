using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyJson;


public class Test : MonoBehaviour
{
    

    [SerializeField] testclass testor1 = new testclass();
    private void Start()
    {
        EasyToJson.ToJson(testor1,"_test01", true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            testor1 = EasyToJson.FromJson<testclass>("_test01");
            Debug.Log(testor1.a);
            testor1.a = 20;
            EasyToJson.ToJson(testor1, "_test01", true);
        }
    }
}
[System.Serializable]
class testclass
{
    public int a = 0;
}
