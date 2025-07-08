using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSound : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip fixedAudioClip;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SpawnSound(AudioClip audioClip)
    {
        SoundFxManager.Instance().SpawnSound(audioClip, transform);

    }
    public void SpawnFixedSound()
    {
        SoundFxManager.Instance().SpawnSound(fixedAudioClip, transform);
    }
}
