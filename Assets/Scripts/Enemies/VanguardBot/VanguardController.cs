using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VanguardController : MonoBehaviour,IEnemy,IHealth
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] LayerMask groundMask;
    [SerializeField] private Transform leftTarget;
    [SerializeField] private Transform rightTarget;

    private Vector3 anchorLeft;
    private Vector3 anchorRight;

    [SerializeField] private Transform rootRay;
    [SerializeField] private Transform rootRayLeft;
    [SerializeField] private Transform rootRayRight;
    private Vector3 rayPointLeft;
    private Vector3 rayPointRight;

    private Vector3 rayPointFrontLeft;
    private Vector3 rayPointFrontRight;
    private Vector3 vectorToFrontLeft;
    private Vector3 vectorToFrontRight;


    [SerializeField] private float lengthStep;
    [SerializeField] private float speedStep;
    private float time;
    [SerializeField] private float stepHeight;
    [SerializeField] private bool isWalkingLeft;
    [SerializeField] private bool isWalkingRight;

    [SerializeField] private float speed;

    [SerializeField] private float speedShake;
    [SerializeField] private float shakeRange;

    [SerializeField] private Vector3 previousPosition;
    [SerializeField] private NavMeshAgent navMeshAgent;
    private Transform playerTransform;

    [Header("Field Of View")]
    [SerializeField] Transform headTransform;
    private Vector3 headDirection;
    private Vector3 directionHeadToPlayer;
    [SerializeField] private int halfViewDegree;
    [SerializeField] private LayerMask masksExceptPlayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float cooldownRotate;
    [SerializeField] private float speedRotate;
    private float cooldownRotateTimer;
    private Vector3 directionRotateTo;
    private Vector3 directionRotateFrom;
    private float rotateAngle;
    private bool isDetectedPlayer;
    private float linearRotate;

    [SerializeField] private float xPatrolArea;
    [SerializeField] private float zPatrolArea;
    private Vector3 destinationPatrol;
    private Vector3 rootPosition;
    [SerializeField] private float timeToUpdateTarget;
    private float updateTargetTimer;
    private Vector3 lastKnowPosition;
    [SerializeField] float rangeToUpdatePosition;
    private bool isGameStarted;

    [Header("Gun")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform leftGunTransform;
    private Vector3 leftGunDirection;
    [SerializeField] private Transform rightGunTransform;
    private Vector3 rightGunDirection;
    [SerializeField] private Transform leftBarrelTransform;
    [SerializeField] private Transform rightBarrelTransform;
    [SerializeField] private float delayShoot;
    private float delayShootTimer;
    [SerializeField]private Vector3 gunRotationOffsetY;
    [SerializeField]
    private Vector3 gunRotationOffsetX;

    [SerializeField] private float circleRadius;
    [SerializeField] private float distanceOfRandomCircle;
    [SerializeField] private float barrelRadius;
    [SerializeField] private AudioClip footStepSound;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] float timeChangePatrol;
    float changePatrolTimer;
    [SerializeField] private GameObject particleExplode;
    [SerializeField] private AudioClip explodeSound;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        headDirection = -headTransform.right;
        leftGunDirection = leftGunTransform.right;
        rightGunDirection = rightGunTransform.right;
        cooldownRotateTimer = cooldownRotate;
        rootPosition = transform.position;
        isGameStarted = true;
        navMeshAgent = GetComponent<NavMeshAgent>();
        RayToGround();
        anchorLeft = rayPointLeft;
        anchorRight = rayPointRight;
        speed = navMeshAgent.speed;
        playerTransform = PlayerInformation.Instance().GetTransform();
    }

    // Update is called once per frame
    void Update()
    {
        RayToGround();
        if (!isDetectedPlayer)
        {
            if (IsSeePlayer())
            {
                isDetectedPlayer = true;
            }
            Patrol();
        }
        else
        {
            Attack();
        }
        
        time = lengthStep / speed;
        if(!isWalkingLeft && Vector3.Distance(anchorLeft, rayPointLeft) > lengthStep / 2.0 && isWalkingRight)
        {
            navMeshAgent.speed = 0;
        }
        else if(!isWalkingRight && Vector3.Distance(anchorRight, rayPointRight) > lengthStep / 2.0 && isWalkingLeft)
        {
            navMeshAgent.speed = 0;
        }
        else
        {
            if(!navMeshAgent.isStopped)
            navMeshAgent.speed = speed;
        }
        if (Vector3.Distance(anchorLeft, rayPointLeft) > lengthStep/2.0 && !isWalkingLeft && !isWalkingRight)
        {
            isWalkingLeft = true;
            StartCoroutine(WalkLeft());
        }
        if(Vector3.Distance(anchorRight,rayPointRight)> lengthStep/2.0 && !isWalkingRight && !isWalkingLeft)
        {
            isWalkingRight = true;
            StartCoroutine(WalkRight());
        }

    }
    private void LateUpdate()
    {
        headTransform.right = -headDirection;
        leftTarget.position = anchorLeft;
        rightTarget.position = anchorRight;
    }
    void Searching()
    {
        cooldownRotateTimer -= Time.deltaTime;
        if (cooldownRotateTimer <= 0)
        {
            cooldownRotateTimer = cooldownRotate;
            rotateAngle = Random.RandomRange(-45, 45);
            directionRotateTo = Quaternion.AngleAxis(rotateAngle, Vector3.up) *(-transform.forward);
            directionRotateFrom = headTransform.right;
            linearRotate = 0;
            Debug.Log("new rotate");
        }
        if(directionRotateTo != Vector3.zero)
        {
            linearRotate = Mathf.Clamp(linearRotate += speedRotate * Time.deltaTime, 0, 1);
            headDirection = -Vector3.Slerp(directionRotateFrom, directionRotateTo, linearRotate).normalized;

        }
        //headTransform.right = -transform.forward;
    }
    void Patrol()
    {
        changePatrolTimer -= Time.deltaTime;
        if (destinationPatrol == Vector3.zero || (transform.position - destinationPatrol).magnitude <= 15f || changePatrolTimer<=0)
        {
            Debug.Log("generate patrol position");
            destinationPatrol.x = rootPosition.x + Random.RandomRange(-xPatrolArea, xPatrolArea);
            destinationPatrol.z = rootPosition.z + Random.RandomRange(-zPatrolArea, zPatrolArea);
            destinationPatrol.y = rootPosition.y;
            RaycastHit hit;
            Physics.Raycast(destinationPatrol, Vector3.down, out hit, Mathf.Infinity, groundLayer);
            destinationPatrol = hit.point;
            NavMeshPath path = new NavMeshPath();
            if (navMeshAgent.CalculatePath(destinationPatrol, path))
            {
                navMeshAgent.SetPath(path);
            }
            else
            {
                //destinationPatrol = Vector3.zero;
            }
            changePatrolTimer = timeChangePatrol;
        }
        Searching();
    }
    void Attack()
    {
        if (IsSeePlayer())
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.speed = 0;
            transform.forward = Vector3.RotateTowards(transform.forward, headDirection, Mathf.Deg2Rad * 10 * Time.deltaTime, 0);
            leftGunDirection = Vector3.RotateTowards(leftGunTransform.right, playerTransform.position+playerTransform.up - leftGunTransform.position, Mathf.Deg2Rad * 90 * Time.deltaTime, 0);
            Quaternion leftTargetRotation = Quaternion.LookRotation(leftGunDirection);
            leftTargetRotation *= Quaternion.Euler(gunRotationOffsetY);
            leftTargetRotation *= Quaternion.Euler(gunRotationOffsetX);
            leftGunTransform.rotation = leftTargetRotation;
            rightGunDirection = Vector3.RotateTowards(rightGunTransform.right, playerTransform.position+playerTransform.up - rightGunTransform.position, Mathf.Deg2Rad * 90 * Time.deltaTime, 0);
            Quaternion rightTargetRotation = Quaternion.LookRotation(rightGunDirection);
            rightTargetRotation *= Quaternion.Euler(gunRotationOffsetY);
            rightTargetRotation *= Quaternion.Euler(-gunRotationOffsetX);
            rightGunTransform.rotation = rightTargetRotation;
            if (delayShootTimer < 0)
            {
                Shoot();
                delayShootTimer = delayShoot;
            }
            else
            {
                delayShootTimer -= Time.deltaTime;
            }
        }
        else
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = speed;
        }
        directionHeadToPlayer = playerTransform.position - headTransform.position;
        headDirection = Vector3.RotateTowards(headDirection, directionHeadToPlayer, Mathf.Deg2Rad * 90 * Time.deltaTime,0);
        headDirection.y = 0;
        ChasePlayer();
    }
    void Shoot()
    {
        Vector3 leftRandomPoint = leftBarrelTransform.position + leftBarrelTransform.forward * distanceOfRandomCircle;
        Vector2 randomInsideCircle = Random.insideUnitCircle * circleRadius;
        leftRandomPoint = leftRandomPoint + leftBarrelTransform.up * randomInsideCircle.y + leftBarrelTransform.right * randomInsideCircle.x;
        Vector3 rightRandomPoint = rightBarrelTransform.position+ rightBarrelTransform.forward * distanceOfRandomCircle;
        randomInsideCircle = Random.insideUnitCircle * circleRadius;
        rightRandomPoint = rightRandomPoint + rightBarrelTransform.up * randomInsideCircle.y + rightBarrelTransform.right * randomInsideCircle.x;
        randomInsideCircle = Random.insideUnitCircle * barrelRadius;
        Instantiate(bullet, leftBarrelTransform.position+ leftBarrelTransform.up * randomInsideCircle.y + leftBarrelTransform.right * randomInsideCircle.x, Quaternion.LookRotation(leftRandomPoint - leftBarrelTransform.position));
        SoundFxManager.Instance().SpawnSound(shootSound, leftBarrelTransform);
        Instantiate(bullet, rightBarrelTransform.position+ rightBarrelTransform.up * randomInsideCircle.y + rightBarrelTransform.right * randomInsideCircle.x, Quaternion.LookRotation(rightRandomPoint - rightBarrelTransform.position));
        SoundFxManager.Instance().SpawnSound(shootSound, rightBarrelTransform);
    }
    void ChasePlayer()
    {
        updateTargetTimer -= Time.deltaTime;
        if (updateTargetTimer <= 0 && (playerTransform.position - lastKnowPosition).magnitude > rangeToUpdatePosition)
        {
            navMeshAgent.destination = playerTransform.position;
            lastKnowPosition = playerTransform.position;
            updateTargetTimer = timeToUpdateTarget;
        }
    }
    bool IsSeePlayer()
    {
        Vector3 vectorToPlayer = playerTransform.position - headTransform.position;
        if (Vector3.Angle(-headTransform.right, vectorToPlayer) <= halfViewDegree && Vector3.Distance(playerTransform.position,transform.position)<=100)
        {
            RaycastHit hit;
            if (Physics.Raycast(headTransform.position, vectorToPlayer, out hit, vectorToPlayer.magnitude, masksExceptPlayer))
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
    void RayToGround()
    {
        RaycastHit hit;
        Physics.Raycast(rootRayLeft.position, rootRayLeft.forward, out hit, Mathf.Infinity, groundMask);
        rayPointLeft = hit.point;
        Physics.Raycast(rootRayRight.position, rootRayRight.forward, out hit, Mathf.Infinity, groundMask);
        rayPointRight = hit.point;
        rayPointFrontLeft = rayPointLeft + vectorToFrontLeft * lengthStep*0.48f;
        rayPointFrontRight = rayPointRight + vectorToFrontRight * lengthStep *0.48f;
    }
    IEnumerator WalkLeft()
    {
        speedStep = 1.0f/time;
        vectorToFrontLeft = (rayPointLeft - anchorLeft).normalized;
        Vector3 oldPosition = anchorLeft;
        Vector3 newPosition = rayPointLeft+transform.forward*lengthStep/2.0f;
        float t = 0;
        do
        {
            t = Mathf.Clamp(t + speedStep * Time.deltaTime, 0, 1);
            anchorLeft = Vector3.Lerp(oldPosition, rayPointFrontLeft,t);
            anchorLeft.y += Mathf.Sin(t * Mathf.PI)*stepHeight;
            yield return null;
        } while (t < 1);
        SoundFxManager.Instance().SpawnSound(footStepSound, newPosition);
        isWalkingLeft = false;
    }
    IEnumerator WalkRight()
    {
        Debug.Log("checkedddd");
        speedStep = 1.0f/time;
        vectorToFrontRight = (rayPointRight - anchorRight).normalized;
        Vector3 oldPosition = anchorRight;
        Vector3 newPosition = rayPointRight + transform.forward * lengthStep / 2.0f;
        float t = 0;
        do
        {
            t = Mathf.Clamp(t + speedStep * Time.deltaTime, 0, 1);
            anchorRight = Vector3.Lerp(oldPosition, rayPointFrontRight, t);
            anchorRight.y += Mathf.Sin(t * Mathf.PI) * stepHeight;
            yield return null;
        } while (t < 1);
        SoundFxManager.Instance().SpawnSound(footStepSound, newPosition);
        isWalkingRight = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        RaycastHit hit;
        Physics.Raycast(rootRayLeft.position, rootRayLeft.forward, out hit, Mathf.Infinity, groundMask);
        Gizmos.DrawSphere(hit.point, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(hit.point, lengthStep/2.0f);
        Physics.Raycast(rootRayRight.position, rootRayRight.forward, out hit, Mathf.Infinity, groundMask);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hit.point, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(hit.point, lengthStep/2.0f);
        Gizmos.color = Color.red;
        if (!isGameStarted)
        {
            Gizmos.DrawLine(transform.position + new Vector3(-xPatrolArea, 0, -zPatrolArea), transform.position + new Vector3(-xPatrolArea, 0, zPatrolArea));
            Gizmos.DrawLine(transform.position + new Vector3(-xPatrolArea, 0, zPatrolArea), transform.position + new Vector3(xPatrolArea, 0, zPatrolArea));
            Gizmos.DrawLine(transform.position + new Vector3(xPatrolArea, 0, zPatrolArea), transform.position + new Vector3(xPatrolArea, 0, -zPatrolArea));
            Gizmos.DrawLine(transform.position + new Vector3(xPatrolArea, 0, -zPatrolArea), transform.position + new Vector3(-xPatrolArea, 0, -zPatrolArea));
        }
        else
        {
            Gizmos.DrawLine(rootPosition + new Vector3(-xPatrolArea, 0, -zPatrolArea), rootPosition + new Vector3(-xPatrolArea, 0, zPatrolArea));
            Gizmos.DrawLine(rootPosition + new Vector3(-xPatrolArea, 0, zPatrolArea), rootPosition + new Vector3(xPatrolArea, 0, zPatrolArea));
            Gizmos.DrawLine(rootPosition + new Vector3(xPatrolArea, 0, zPatrolArea), rootPosition + new Vector3(xPatrolArea, 0, -zPatrolArea));
            Gizmos.DrawLine(rootPosition + new Vector3(xPatrolArea, 0, -zPatrolArea), rootPosition + new Vector3(-xPatrolArea, 0, -zPatrolArea));
        }
        Gizmos.DrawSphere(destinationPatrol, 2);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(headTransform.position, transform.position + directionRotateTo*20);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(headTransform.position, transform.position + headDirection * 20);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Explode();
        }
        isDetectedPlayer = true;
    }
    void Explode()
    {
        Instantiate(particleExplode, transform.position, Quaternion.identity).transform.localScale *= 10;
        SoundFxManager.Instance().SpawnSound(explodeSound, transform.position);
        Destroy(gameObject);
    }

    public void IncreaseHealth(int amount)
    {
        
    }

    public void DecreaseHealth(int amount)
    {
        currentHealth -= amount;
        if(isDetectedPlayer == false)
        {
            isDetectedPlayer = true;
        }
        if (currentHealth <= 0)
        {
            Explode();
        }
    }
}
