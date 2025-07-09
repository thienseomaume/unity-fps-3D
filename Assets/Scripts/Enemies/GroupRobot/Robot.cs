using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Robot : Enemy
{
    [HideInInspector]public Group group;
    public Animator animator;
    public float maxSpeed;
    private float currentSpeed;
    public float groupAlignmentErrorMax;
    public Vector3 velocity = Vector3.zero;
    public Vector3 position => transform.position;
    public Vector3 direction { get; private set; }
    public Transform viewPoint;
    public NavMeshAgent navMeshAgent;
    public float fireCooldown;
    public float halfOfView;
    public float maxDetectRange;
    public LayerMask obstacleLayer;
    public float maxPatrolX;
    public float maxPatrolZ;
    private Vector3 startPosition;
    public float speedRotateToTarget;
    public float cosMinSeeTarget = 0.98f;
    public float searchingTime;

    public float shootingError;
    public Transform barrelOfGun;
    public ParticleSystem muzzleFlash;
    public LayerMask interactableLayer;
    public AudioClip impactDirt;
    public int damage;

    public int maxHealth;
    private int curentHealth;
    public GameObject destroyEffect;
    public AudioClip destroySound;
    public bool HasGroup()
    {
        return group != null;
    }
    public bool IsGroupLeader()
    {
        if(group == null)
        {
            return false;
        }
        return group.IsLeader(this);
    }
    public bool IsGroupFollower()
    {
        return IsGroupLeader() == false;
    }
    public void AnimCrossFade(int animation, float transition)
    {
        animator.CrossFadeInFixedTime(animation, transition);
    }
    public void AnimInstant(int animation)
    {
        animator.Play(animation,0,0);
    }
    public void AnimInstant(int animation,float normalizedTime)
    {
        animator.Play(animation,0,normalizedTime);
    }
    public bool AnimCurrentIs(int animation)
    {
        AnimatorStateInfo stateInfor = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfor.shortNameHash == animation;
    }
    public bool IsCurrentAnimStop()
    {
        AnimatorStateInfo stateInfor = animator.GetCurrentAnimatorStateInfo(0);
        bool check = stateInfor.IsName("Aiming");
        if (check)
        {
            Debug.Log("check");
        }
        Debug.Log(stateInfor.normalizedTime);
        if(stateInfor.normalizedTime>=1.0){
            return true;
        }
        else
        {
            return false;
        }
    }
    public AnimatorStateInfo GetNextSate()
    {
        return animator.GetNextAnimatorStateInfo(0);
    }
    public Vector3 GetStartPosition()
    {
        return startPosition;
    }
    public void Attack(Transform target)
    {
        Vector3 directionToTarget = (target.position - barrelOfGun.position).normalized;
        Vector3 horizontal = Vector3.Cross(directionToTarget, Vector3.up).normalized;
        Vector3 vertical = Vector3.Cross(directionToTarget, -horizontal).normalized;
        Vector3 point = barrelOfGun.position + directionToTarget * 10 + horizontal * Random.Range(-shootingError, shootingError) + vertical * Random.Range(0, shootingError);
        RaycastHit hit;
        Physics.Raycast(barrelOfGun.position, point - barrelOfGun.position, out hit, Mathf.Infinity, interactableLayer);
        Collider interactionCollider = hit.collider;
        HumanoidBulletScript bullet = HumanoidBulletPool.Instance().GetBullet();
        bullet.transform.position = barrelOfGun.position;
        bullet.gameObject.SetActive(true);
        if (interactionCollider != null)
        {
            bullet.setBegin(barrelOfGun.position, hit.point);
            if (interactionCollider.GetComponent<IHealth>() == null)
            {
                Debug.Log("ihealth component is null");
                GameObject impactClone = BulletImpactManager.Instance().dirtImpactPool.Dequeue();
                if (impactClone == null)
                {
                    Debug.Log("impact is null");
                }
                impactClone.transform.position = hit.point;
                impactClone.transform.rotation = Quaternion.LookRotation(hit.normal);
                impactClone.SetActive(true);
                SoundFxManager.Instance().SpawnSound(impactDirt, hit.point);
                BulletImpactManager.Instance().dirtImpactPool.Enqueue(impactClone);
            }
            else
            {
                interactionCollider.GetComponent<IHealth>().DecreaseHealth(damage);
                Debug.Log("checked hit");
            }
        }
        else
        {
            bullet.setBegin(barrelOfGun.position, (point - barrelOfGun.position).normalized * 999);
        }
        muzzleFlash.Play();

    }
    public override void IncreaseHealth(int amount)
    {
        
    }
    public override void DecreaseHealth(int amount)
    {
        curentHealth -= amount;
        if (curentHealth <= 0)
        {
            GameObject.Instantiate(destroyEffect,transform.position,Quaternion.identity);
            SoundFxManager.Instance().SpawnSound(destroySound, transform.position);
            if (HasGroup())
            {
                group.RemoveMember(this);
            }
            Destroy(gameObject);
        }
    }
    private void Awake()
    {
        startPosition = transform.position;
        curentHealth = maxHealth;
    }
    private void Start()
    {
        HumanoidBulletPool.Instance().ScalePool();
    }
    private void Update()
    {
        direction = transform.forward;
        navMeshAgent.speed = maxSpeed / 2.0f;

        if (HasGroup())
        {
            float groupAlignmentError = Vector3.Distance(navMeshAgent.nextPosition, group.formation.GetPosition(group, this));
            if (groupAlignmentError > 0.5f)
            {
                navMeshAgent.speed += (maxSpeed / 2.0f) * (float)((groupAlignmentError - 0.5f) / groupAlignmentErrorMax);
            }
        }

        currentSpeed = navMeshAgent.velocity.magnitude;
        animator.SetFloat("Speed", currentSpeed / maxSpeed);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if(startPosition == Vector3.zero)
        {
            Gizmos.DrawLine(transform.position + transform.forward * maxPatrolZ / 2 - transform.right * maxPatrolX / 2, transform.position + transform.forward * maxPatrolZ / 2 + transform.right * maxPatrolX / 2);
            Gizmos.DrawLine(transform.position - transform.forward * maxPatrolZ / 2 - transform.right * maxPatrolX / 2, transform.position - transform.forward * maxPatrolZ / 2 + transform.right * maxPatrolX / 2);
            Gizmos.DrawLine(transform.position - transform.forward * maxPatrolZ / 2 - transform.right * maxPatrolX / 2, transform.position + transform.forward * maxPatrolZ / 2 - transform.right * maxPatrolX / 2);
            Gizmos.DrawLine(transform.position - transform.forward * maxPatrolZ / 2 + transform.right * maxPatrolX / 2, transform.position + transform.forward * maxPatrolZ / 2 + transform.right * maxPatrolX / 2);
        }
        else
        {
            Gizmos.DrawLine(startPosition + Vector3.forward * maxPatrolZ / 2 - Vector3.right * maxPatrolX / 2, startPosition + Vector3.forward * maxPatrolZ / 2 + Vector3.right * maxPatrolX / 2);
            Gizmos.DrawLine(startPosition - Vector3.forward * maxPatrolZ / 2 - Vector3.right * maxPatrolX / 2, startPosition - Vector3.forward * maxPatrolZ / 2 + Vector3.right * maxPatrolX / 2);
            Gizmos.DrawLine(startPosition - Vector3.forward * maxPatrolZ / 2 - Vector3.right * maxPatrolX / 2, startPosition + Vector3.forward * maxPatrolZ / 2 - Vector3.right * maxPatrolX / 2);
            Gizmos.DrawLine(startPosition - Vector3.forward * maxPatrolZ / 2 + Vector3.right * maxPatrolX / 2, startPosition + Vector3.forward * maxPatrolZ / 2 + Vector3.right * maxPatrolX / 2);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawLine(viewPoint.position, viewPoint.position + viewPoint.up*5);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + direction * 5);
    }
}
