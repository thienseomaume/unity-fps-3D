using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundObjectScript : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;
    private AudioClip audioClip;
    private Transform transformSpawn;

    public void SetUp(AudioClip audioClip, bool loop)
    {
        this.audioClip = audioClip;
        audioSource.loop = loop;
        transformSpawn = null;
    }
    public void SetUp(AudioClip audioClip, bool loop, Transform transformSpawn)
    {
        this.audioClip = audioClip;
        audioSource.loop = loop;
        this.transformSpawn = transformSpawn;
    }
    private void OnEnable()
    {
        if(audioClip != null)
        {
            if (audioSource.loop)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
            }
            else
            {
                audioSource.PlayOneShot(audioClip);
            }
            if (transformSpawn != null)
            {
                transform.position = transformSpawn.position;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (audioSource.isPlaying == false)
        {
            gameObject.SetActive(false);
        }
        if(transformSpawn != null)
        {
            if (transformSpawn.gameObject.activeSelf == false)
            {
                gameObject.SetActive(false);
            }
            transform.position = transformSpawn.position;
        }
    }
    private void OnDisable()
    {
        SoundFxManager.Instance().GetBackToList(gameObject);
    }
}
