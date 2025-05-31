using System.Collections.Generic;
using UnityEngine;

public class SoundFxManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Queue<GameObject> listSoundObjects = new Queue<GameObject>();
    [SerializeField] private GameObject soundObject;
    public int numberOfSound;
    private static SoundFxManager instance;
    
    public static SoundFxManager Instance()
    {
        return instance;
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        for(int i = 0; i < numberOfSound; i++)
        {
            GameObject cloneSoundObject = Instantiate(soundObject, transform, true);
            cloneSoundObject.SetActive(false);
            listSoundObjects.Enqueue(cloneSoundObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnSound(AudioClip audioClip,bool loop,Transform transformSpawn) {
        if (Vector3.Distance(transformSpawn.position, PlayerInformation.Instance().GetTransform().position) >= 100)
        {
            return;
        }
        GameObject audioObject;
        if (listSoundObjects.TryDequeue(out audioObject))
        {
            SoundObjectScript soundScript = audioObject.GetComponent<SoundObjectScript>();
            soundScript.SetUp(audioClip, loop,transformSpawn);
            audioObject.SetActive(true);
        }
        
    }
    public void SpawnSound(AudioClip audioClip,bool loop, Vector3 positionSpawn)
    {
        if (Vector3.Distance(positionSpawn, PlayerInformation.Instance().GetTransform().position) >= 100)
        {
            return;
        }
        GameObject audioObject;
        if (listSoundObjects.TryDequeue(out audioObject))
        {
            SoundObjectScript soundScript = audioObject.GetComponent<SoundObjectScript>();
            soundScript.SetUp(audioClip, loop);
            audioObject.transform.position = positionSpawn;
            audioObject.SetActive(true);
        }
    }
    public void GetBackToList(GameObject soundObject)
    {
        listSoundObjects.Enqueue(soundObject);
    }
}
