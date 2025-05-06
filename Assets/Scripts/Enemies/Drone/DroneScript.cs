using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneScript : MonoBehaviour,IEnemy,IHealth
{
    // Start is called before the first frame update
    private Rigidbody droneRigidbody;
    [SerializeField] int maxHealth;
    [SerializeField] int currentHealth;
    [SerializeField] private float amplitude;
    [SerializeField] private float speedRotateAngel;
    [SerializeField] private float speedToRoot;

    private float angle;

    public DronesController dronesController;
    private float rootHeight;
    private Vector2 rootXZ;
    private Vector3 rootDirection;

    public Transform playerTransform;

    [Header("Field Of View")]
    [SerializeField] private int halfViewDegree;
    [SerializeField] private LayerMask masksExceptPlayer;
    [SerializeField] private float cooldownRotate;
    [SerializeField] private float speedRotate;
    private float cooldownRotateTimer;
    private Vector3 directionRotateTo;
    private Vector3 directionRotateFrom;
    private float rotateAngle;
    private float t;

    [Header("attack")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gun;
    [SerializeField] private ParticleSystem muzzleFlash;

    [SerializeField] private float attackCooldown;
    private float attackCooldownTimer;
    [SerializeField] private float delayShoot;
    [SerializeField] private float delayShootTimer;
    [SerializeField] private int maxShootPerAttack;
    [SerializeField] private float shootCounter;
    [SerializeField] private AudioClip gunSound;
    [SerializeField] GameObject destroyEffect;
    [SerializeField] AudioClip destroySound;
    void Start()
    {
        playerTransform = PlayerInformation.Instance().GetTransform();
        droneRigidbody = GetComponent<Rigidbody>();
        rootHeight = transform.position.y;
        rootXZ = new Vector2(transform.position.x, transform.position.z);
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        UpDownFluctuate();
        UpdateXZ();
        NotifyToController();
        if (!dronesController.playerDetected)
        {
            Searching();
            delayShootTimer = 1.0f;
        }
        else
        {
            Attacking();
        }
    }
    void Searching()
    {
        cooldownRotateTimer -= Time.deltaTime;
        if (cooldownRotateTimer <= 0)
        {
            cooldownRotateTimer = cooldownRotate;
            rotateAngle = Random.RandomRange(-45, 45);
            directionRotateTo = Quaternion.AngleAxis(rotateAngle, Vector3.up) * rootDirection;
            directionRotateFrom = transform.forward;
            t = 0;
        }
        transform.rotation = Quaternion.LookRotation(Vector3.Slerp(directionRotateFrom,directionRotateTo, t+=speedRotate * Time.deltaTime));
    }
    void Attacking()
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        Quaternion currenQuaternion = transform.rotation;
        transform.rotation = Quaternion.RotateTowards(currenQuaternion, Quaternion.LookRotation(directionToPlayer),speedRotate*400*Time.deltaTime);
        
        if(attackCooldownTimer<=0)
        {
            if (delayShootTimer > 0)
            {
                delayShootTimer -= Time.deltaTime;
            }
            else
            {
                Instantiate(bullet, gun.position,gun.rotation);
                muzzleFlash.Play();
                SoundFxManager.Instance().SpawnSound(gunSound, gun);
                delayShootTimer = delayShoot;
                shootCounter -= 1;
            }
        }
        else
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        if (shootCounter <= 0)
        {
            attackCooldownTimer = attackCooldown;
            shootCounter = maxShootPerAttack;
        }
    }
    void UpDownFluctuate()
    {
        angle = Mathf.Repeat(angle+speedRotateAngel * Time.deltaTime,360f);
        transform.position = new Vector3(transform.position.x, rootHeight + Mathf.Sin(angle)*amplitude, transform.position.z);
    }
    void UpdateXZ()
    {
        Vector3 directionToRoot = new Vector3(rootXZ.x, 0, rootXZ.y) - transform.position;
        directionToRoot.y = 0f;
        droneRigidbody.velocity=directionToRoot * speedToRoot;
    }
    public void setRootXZ(Vector2 xzVector)
    {
        rootXZ = xzVector;
    }
    public void setRootDirection(Vector3 direction)
    {
        rootDirection = direction;
    }
    void NotifyToController()
    {
        if(dronesController != null)
        {
            //Debug.Log("checked");
            if(dronesController.playerDetected == false && IsSeePlayer())
            {
                dronesController.playerDetected = true;
            }
        }
    }
    bool IsSeePlayer()
    {
        Vector3 vectorToPlayer = playerTransform.position - transform.position;
       // Debug.Log("degree=" + Vector3.Angle(transform.forward, vectorToPlayer));
        if (Vector3.Angle(transform.forward, vectorToPlayer) <= halfViewDegree && vectorToPlayer.magnitude<=100)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, vectorToPlayer,out hit, vectorToPlayer.magnitude,masksExceptPlayer))
            {
                Debug.Log(hit.collider.name);
                return false;
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + directionRotateTo);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + rootDirection.normalized*5);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 50 + transform.up * 50);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 50 - transform.up * 50);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 50 + transform.right * 50);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 50 - transform.right * 50);
    }

    public void TakeDamage(int damage)
    {
        if (!dronesController.playerDetected)
        {
            dronesController.playerDetected = true;
        }
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
        Destroy(dronesController.gameObject);
        Destroy(gameObject);
    }

    public void IncreaseHealth(int amount)
    {
        
    }

    public void DecreaseHealth(int amount)
    {
        if (!dronesController.playerDetected)
        {
            dronesController.playerDetected = true;
        }
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Explode();
        }
    }
}
