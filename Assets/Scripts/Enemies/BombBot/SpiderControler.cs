using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderControler : MonoBehaviour, IEnemy,IHealth
{
    // Start is called before the first frame update
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private float speedToAnimator;
    private NavMeshAgent navMeshAgent;
    public Transform playerTransform;
    private Animator animator;

    [SerializeField] private float distanceToStop;
    [SerializeField] private float explodeRange;
    [SerializeField] private float timeToExplode;
    [SerializeField] private float timeChangeColor;
    private float changeColorTimer;
    [SerializeField] private GameObject particleExplode;
    [SerializeField] private int damage;
    private bool isReadyExplode;

    private SkinnedMeshRenderer renderer;
    private Material eyeMaterial;
    private bool isRed;
    [SerializeField] private AudioClip explodeSound;
    private AudioSource audioSource;
    [SerializeField] private AudioClip footStep;
    [SerializeField] private AudioClip explodeCountDown;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.speed = navMeshAgent.speed / speedToAnimator;
        currentHealth = maxHealth;
        renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        eyeMaterial = renderer.materials[2];
        isRed = true;
        playerTransform = PlayerInformation.Instance().GetTransform();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = footStep;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= distanceToStop && !isReadyExplode)
        {
            isReadyExplode = true;
            animator.speed = 0;
            audioSource.clip = explodeCountDown;
            audioSource.Play();
            navMeshAgent.SetDestination(transform.position);
        }
        if (isReadyExplode)
        {
            
            ReadyExplode();
        }
        else
        {
            Chasing();
        }
    }
    void Chasing()
    {
        navMeshAgent.SetDestination(playerTransform.position);

    }
    void ReadyExplode()
    {
        timeToExplode -= Time.deltaTime;
        if (changeColorTimer >= 0)
        {
            changeColorTimer -= Time.deltaTime;
        }
        if (changeColorTimer <= 0)
        {
            if (isRed)
            {
                isRed = false;
                changeColorTimer = timeChangeColor;
                eyeMaterial.SetColor("_EmissionColor", Color.white);
            }
            else
            {
                isRed = true;
                changeColorTimer = timeChangeColor;
                eyeMaterial.SetColor("_EmissionColor", Color.red);
            }
        }
        if (timeToExplode <= 0)
        {
            Explode();
        }
    }
    void Explode()
    {
        Debug.Log("exploded");
        Instantiate(particleExplode, transform.position, Quaternion.identity).transform.localScale *= 3;
        if (Vector3.Distance(transform.position,playerTransform.position)<= explodeRange)
        {
            playerTransform.GetComponent<PlayerController>().TakeDamage(damage);
        }
        SoundFxManager.Instance().SpawnSound(explodeSound, transform.position);
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Explode();
        }
    }

    public void IncreaseHealth(int amount)
    {
        
    }

    public void DecreaseHealth(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Explode();
        }
    }
}
