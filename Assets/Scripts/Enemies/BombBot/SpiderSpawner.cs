using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderSpawner : MonoBehaviour,IEnemy, IHealth
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    [SerializeField] private Transform spawnTransform;
    [SerializeField] private float spawnCooldown;
    [SerializeField] private float spawnCooldownTimer;
    [SerializeField] private GameObject spider;
    [SerializeField] ParticleSystem spawnEffect;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private float playerDetectRange;
    [SerializeField] private AudioClip spawnEffectSound;
    [SerializeField] private AudioClip appearEffect;
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private AudioClip destroySound;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = PlayerInformation.Instance().GetTransform();
        spawnCooldownTimer = spawnCooldown;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpawnCooldown();
        if (PlayerInside())
        {
            if(spawnCooldownTimer <= 0)
            {
                Instantiate(spider, spawnTransform.position, Quaternion.identity);
                spawnCooldownTimer = spawnCooldown;
            }else if (spawnCooldownTimer>1 && spawnCooldownTimer <= 1.5 && spawnEffect.isStopped)
            {
                spawnEffect.Play();
                SoundFxManager.Instance().SpawnSound(spawnEffectSound, spawnTransform.position);
            }
        }
    }
    bool PlayerInside()
    {
        if(Vector3.Distance(transform.position, playerTransform.position) <= playerDetectRange)
        {
            return true;
        }
        return false;
    }
    void UpdateSpawnCooldown()
    {
        if (spawnCooldownTimer > 0)
        {
            spawnCooldownTimer -= Time.deltaTime;
        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Explode();
        }
    }
    void Explode()
    {
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
        SoundFxManager.Instance().SpawnSound(destroySound, transform.position);
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, playerDetectRange);
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
