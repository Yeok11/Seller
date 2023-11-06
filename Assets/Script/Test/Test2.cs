using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    float a = 0;
    private void Update()
    {
        a += Time.deltaTime;
        //Debug.Log("a "+ a);

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        //b();

        Debug.Log((float)(0 / 10));

        for (int i = 0; i < 3; i++)
        {
            Debug.Log("3");
            for (int  j= 0; j < 4; j++)
            {
                Debug.Log("2");
                //break;

                goto skip;
            }
        }

    skip:
        Debug.Log("³¡");
    }

    float c = 0;

    void b()
    {
        while (true)
        {
            c += Time.deltaTime;
            Debug.Log("c " + c);

            if (c > 100) break;
        }
    }
}
