using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject[] IamSeoJang�̿���;
    int cnt = 0;

    [SerializeField] string[] Mes;
    [SerializeField] TextMeshProUGUI pos;

    private void Awake()
    {
        for (int i = 0; i < IamSeoJang�̿���.Length; i++)
        {
            IamSeoJang�̿���[i].SetActive(false);
            pos.SetText(Mes[cnt]);
        }

        //Mes = new string[IamSeoJang�̿���.Length];
        IamSeoJang�̿���[0].SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) end();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            cnt++;
            if (cnt >= IamSeoJang�̿���.Length)
            {
                end();
                return;
            }
            pos.SetText(Mes[cnt]);
            IamSeoJang�̿���[cnt].SetActive(true);
            IamSeoJang�̿���[cnt - 1].SetActive(false);
            
        }
    }

    void end()
    {
        SceneManager.LoadScene("Title");
    }
}
