using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerNewMap : MonoBehaviour,IInteractable
{
    // Start is called before the first frame update
    [SerializeField]string nextMap;
    [SerializeField] Vector3 playerSpawnPosition;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        foreach(var objectFound in FindObjectsOfType(typeof(MonoBehaviour)))
        {
            if(objectFound is IEnemy)
            {
                Debug.Log("found an object");
                return;
            }
        }
        GameManager.Instance().NextLevel(nextMap, playerSpawnPosition);
    }
}
