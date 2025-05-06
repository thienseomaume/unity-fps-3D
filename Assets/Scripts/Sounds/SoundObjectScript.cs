using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundObjectScript : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;
    public AudioClip audioClip;
    public Transform transformSpawn;
    private void Awake()
    {
        audioSource.loop = false;
    }
    private void OnEnable()
    {
        if (audioClip != null)
        {
            if (transformSpawn != null)
            {
                transform.position = transformSpawn.position;
            }
            audioSource.PlayOneShot(audioClip, GameManager.Instance().GetSetting().soundFXVolume);
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource.isPlaying == false)
        {
            transformSpawn = null;
            SoundFxManager.Instance().GetBackToList(gameObject);
            gameObject.SetActive(false);
        }
        if (transformSpawn != null)
        {
            transform.position = transformSpawn.position;
        }
    }
}
