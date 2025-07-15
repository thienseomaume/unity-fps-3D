using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImpactScript : MonoBehaviour
{
    public Queue<GameObject> queueOwner;
    private ParticleSystem particle;
    public bool isPlaying;
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlaying)
        {
            if(particle.isPlaying == false)
            {
                queueOwner.Enqueue(this.gameObject);
                isPlaying = false;
                gameObject.SetActive(false);
            }
        }
    }
}
