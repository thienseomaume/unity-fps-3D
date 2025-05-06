using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlLoopSoundfx : MonoBehaviour
{
    // Start is called before the first frame update
    AudioSource audiosource;
    float volume;
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (volume != GameManager.Instance().GetSetting().soundFXVolume)
        {
            volume = GameManager.Instance().GetSetting().soundFXVolume;
            audiosource.volume = volume;
        }
    }
}
