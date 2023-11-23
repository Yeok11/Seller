using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Test2 : MonoBehaviour
{
    public AudioSource[] audios;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) audios[0].Play();
        if (Input.GetKeyDown(KeyCode.Alpha2)) audios[1].Play();
        if (Input.GetKeyDown(KeyCode.Alpha3)) audios[2].Play();
    }
}
