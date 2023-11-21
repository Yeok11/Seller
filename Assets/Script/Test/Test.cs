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
            Debug.Log("��");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(tester());
            Debug.Log("�����");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            StopAllCoroutines();
            Debug.Log("��� ��");
        }
    }

    IEnumerator tester()
    {
        int i = 1;
        while (true)
        {
            yield return new WaitForSeconds(1);
            Debug.Log(i + "��");
            i++;
        }
    }
}
