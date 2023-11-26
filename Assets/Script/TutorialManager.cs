using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject[] IamSeoJang이에요;
    int cnt = 0;

    [SerializeField] string[] Mes;
    [SerializeField] TextMeshProUGUI pos;

    private void Awake()
    {
        for (int i = 0; i < IamSeoJang이에요.Length; i++)
        {
            IamSeoJang이에요[i].SetActive(false);
            pos.SetText(Mes[cnt]);
        }

        //Mes = new string[IamSeoJang이에요.Length];
        IamSeoJang이에요[0].SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) end();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            cnt++;
            if (cnt >= IamSeoJang이에요.Length)
            {
                end();
                return;
            }
            pos.SetText(Mes[cnt]);
            IamSeoJang이에요[cnt].SetActive(true);
            IamSeoJang이에요[cnt - 1].SetActive(false);
            
        }
    }

    void end()
    {
        SceneManager.LoadScene("Title");
    }
}
