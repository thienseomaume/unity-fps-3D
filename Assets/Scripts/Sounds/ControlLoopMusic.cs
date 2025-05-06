using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlLoopMusic : MonoBehaviour
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
        if(volume != GameManager.Instance().GetSetting().musicVolume)
        {
            volume = GameManager.Instance().GetSetting().musicVolume;
            audiosource.volume = volume;
        }
    }
}
