using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour, IEnemy,IHealth
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackCooldown;
    private float attackCooldownTimer;
    [SerializeField] private float launchCooldown;
    private float launchCooldownTimer;
    [SerializeField] private int numberOfRockerPerAttack;
    private int rocketCounter;
    [SerializeField] private Transform body;
    [SerializeField] private Transform leftSpawn;
    [SerializeField] private Transform rightSpawn;
    [SerializeField] private GameObject rocket;
    private Transform playerTransform;
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private AudioClip destroySound;
    
    void Start()
    {
        rocketCounter = numberOfRockerPerAttack;
        playerTransform = PlayerInformation.Instance().GetTransform();
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Math.Clamp(currentHealth - damage, 0, maxHealth);
        if(currentHealth == 0)
        {
            Explode();
        }
    }
    void Explode()
    {
        Instantiate(destroyEffect, transform.position, Quaternion.identity).transform.localScale = transform.localScale;
        SoundFxManager.Instance().SpawnSound(destroySound, transform.position);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {

        if (CheckPlayerInside())
        {

            LookAtPlayer();
            if (attackCooldownTimer <= 0)
            {

                AttackPlayer();
            }
        }
        UpdateCooldown();
    }
    bool CheckPlayerInside()
    {
        Vector3 vectorToPlayer = playerTransform.position - this.transform.position;
        Vector2 vector2ToPlayer = new Vector2(vectorToPlayer.x, vectorToPlayer.z);
        if (vector2ToPlayer.magnitude < attackRadius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void AttackPlayer()
    {
        if (rocketCounter % 2 == 1)
        {
            SpawnRocket(leftSpawn);
        }
        else
        {
            SpawnRocket(rightSpawn);
        }
    }
    void UpdateCooldown()
    {
        if (rocketCounter == 0)
        {
            attackCooldownTimer = attackCooldown;
            rocketCounter = numberOfRockerPerAttack;
        }
        if (attackCooldownTimer >0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        if (launchCooldownTimer > 0)
        {
            launchCooldownTimer -= Time.deltaTime;
        }
    }
    void SpawnRocket(Transform spawnPoit)
    {
        if(launchCooldownTimer<=0)
        {
            Instantiate(rocket, spawnPoit.position,Quaternion.identity).GetComponent<RocketScript>().SetPlayerTransform(playerTransform);
            rocketCounter -= 1;
            launchCooldownTimer = launchCooldown;
        }
    }
    void LookAtPlayer()
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        body.rotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0f, directionToPlayer.z));
    }

    public void IncreaseHealth(int amount)
    {
        
    }

    public void DecreaseHealth(int amount)
    {
        currentHealth = Math.Clamp(currentHealth - amount, 0, maxHealth);
        if (currentHealth == 0)
        {
            Explode();
        }
    }
}
