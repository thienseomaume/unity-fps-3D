using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    private Transform playerTransform;

    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private float explodeRadius;
    [SerializeField] private GameObject bodyRocket;
    [SerializeField] private float timeWaitSmoke;
    [SerializeField] private bool destroyed;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] private GameObject explodeParticle;
    private GameObject explodeClone;
    private float speedRotate;
    AudioSource audioSource;
    [SerializeField] AudioClip explodeSound;
    // Start is called before the first frame update
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = GameManager.Instance().GetSetting().soundFXVolume;
    }
    void Start()
    {
        destroyed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransform != null && !destroyed)
        {
            
            Vector3 directionToPlayer = playerTransform.position - transform.position;
            directionToPlayer = directionToPlayer.normalized;
            if (transform.up.y < 0)
            {
                speedRotate = 8*Time.deltaTime;
            }
            else
            {
                speedRotate = 0.5f * Time.deltaTime;
            }
            Vector3 moveDirection = Vector3.Slerp(transform.up, directionToPlayer,speedRotate);
            transform.up = moveDirection;
            transform.position += speed * transform.up * Time.deltaTime;
        }
        if (Physics.CheckSphere(transform.position, 1, groundLayer) && !destroyed)
        {
            Debug.Log("interract ground");
            DestroyRocketBody();
        }
        if (destroyed)
        {
            timeWaitSmoke -= Time.deltaTime;
        }
        if (timeWaitSmoke<=0 && destroyed)
        {
            Destroy(explodeClone);
            Destroy(gameObject);
        }
    }
     void DestroyRocketBody()
    {
        bodyRocket.SetActive(false);
        destroyed = true;
        explodeClone = Instantiate(explodeParticle, transform.position, Quaternion.identity);
        if ((playerTransform.position - transform.position).magnitude <= explodeRadius)
        {
            playerTransform.GetComponent<PlayerController>().TakeDamage(damage);
        }
        audioSource.Stop();
        SoundFxManager.Instance().SpawnSound(explodeSound, transform.position);

    }
    public void SetPlayerTransform(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }
    private void OnDrawGizmos()
    {
        if (playerTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, playerTransform.position - transform.position);
        }
    }
}
