using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test2 : MonoBehaviour
{
    int a;
    Scene ak = new Scene();

    private void Start()
    {
        Debug.Log(a++);

       //    SceneManager.SetActiveScene(ak);

        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            PlayerPrefs.Save();
            SceneManager.LoadScene(2);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            //PlayerPrefs.Save();
            SceneManager.LoadScene(2);
        }
    }
}
