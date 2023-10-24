using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    float a = 0;
    private void Update()
    {
        a += Time.deltaTime;
        Debug.Log("a "+ a);

    }

    private void Start()
    {
        b();
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
