using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(tester());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StopCoroutine("tester");
            Debug.Log("ÄÆ");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(tester());
            Debug.Log("Àç½ÃÀÛ");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            StopAllCoroutines();
            Debug.Log("¸ðµÎ ÄÆ");
        }
    }

    IEnumerator tester()
    {
        int i = 1;
        while (true)
        {
            yield return new WaitForSeconds(1);
            Debug.Log(i + "ÃÊ");
            i++;
        }
    }
}
