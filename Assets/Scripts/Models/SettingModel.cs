using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingModel
{
    public float soundFXVolume;
    public float musicVolume;

    public SettingModel()
    {
    }

    public SettingModel(float soundFXVolume, float musicVolume)
    {
        this.soundFXVolume = soundFXVolume;
        this.musicVolume = musicVolume;
    }
}
