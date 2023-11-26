using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : SingleTon<AudioManager>
{
    [SerializeField] private GameObject[] Sounds;
    [SerializeField] internal AudioSource[] audiosource; // �⺻ Bgm, ���� Bgm, �� eff, �� eff

    public AudioMixer mixer;

    //static bool SliderValueSet = false;

    static float[] SliderValue = { -20, 10, 10 };

    private void Start()
    {
        Sounds[0].GetComponentInChildren<Slider>().value = SliderValue[0];
        Sounds[1].GetComponentInChildren<Slider>().value = SliderValue[1];
        Sounds[2].GetComponentInChildren<Slider>().value = SliderValue[2];

        for (int i = 0; i < Sounds.Length; i++) AudioCtrl(i);
    }

    public void AudioCtrl(int num)
    {
        SliderValue[num] = Sounds[num].GetComponentInChildren<Slider>().value;

        switch (num)
        {
            case 0:
                if (SliderValue[num] == -40f) mixer.SetFloat("Master", -80);
                else mixer.SetFloat("Master", SliderValue[num]);
                break;

            case 1:
                if (SliderValue[num] == -40f) mixer.SetFloat("BGM", -80);
                else mixer.SetFloat("BGM", SliderValue[num]);
                break;

            case 2:
                if (SliderValue[num] == -40f) mixer.SetFloat("Effect", -80);
                else mixer.SetFloat("Effect", SliderValue[num]);
                break;
        }
        try
        {
            Sounds[num].transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(Mathf.RoundToInt((SliderValue[num] + 40) / (num == 0 ? 40 : 50) * 100) + "%");
        }
        catch
        {
            Sounds[num].transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(Mathf.RoundToInt((SliderValue[num] + 40) / (num == 0 ? 40 : 50) * 100) + "%");
        }
        
    }

    public void SoundOnOff()
    {
        Toggle soundtoggle = Sounds[0].GetComponentInChildren<Toggle>();

        audiosource[0].mute = soundtoggle.isOn ? !(Sounds[1].GetComponentInChildren<Toggle>().isOn) : true; //�⺻ �����
        audiosource[1].mute = soundtoggle.isOn ? !(Sounds[1].GetComponentInChildren<Toggle>().isOn) : true; //���� ��
        audiosource[2].mute = soundtoggle.isOn ? !(Sounds[2].GetComponentInChildren<Toggle>().isOn) : true; //�� �Ҹ�
        audiosource[3].mute = soundtoggle.isOn ? !(Sounds[2].GetComponentInChildren<Toggle>().isOn) : true; //�� �Ҹ�
    }
}
