using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImpactManager : MonoBehaviour
{
    [SerializeField] private GameObject dirtImpact;
    [SerializeField] private GameObject metalImpact;
    public Queue<GameObject> dirtImpactPool = new Queue<GameObject>();
    public Queue<GameObject> metalImpactPool = new Queue<GameObject>();
    private static BulletImpactManager instance;
    public static BulletImpactManager Instance()
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
    private void Start()
    {
        CreatePool();
    }
    private void CreatePool()
    {
        for (int i = 0; i < 32; i++)
        {
            GameObject clone = Instantiate(dirtImpact);
            clone.transform.SetParent(transform);
            dirtImpactPool.Enqueue(clone);
            clone.SetActive(false);
        }
        for (int i = 0; i < 32; i++)
        {
            GameObject clone = Instantiate(metalImpact);
            clone.transform.SetParent(transform);
            metalImpactPool.Enqueue(clone);
            clone.SetActive(false);
        }

    }

}

