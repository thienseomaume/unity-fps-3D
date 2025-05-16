using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Robot : Enemy
{
    [HideInInspector]public Group group;
    public Animator animator;
    public float speed;
    public Vector3 velocity = Vector3.zero;
    public bool alive;
    public bool wander;
    public Vector3 position => transform.position;
    public Vector3 direction { get; private set; }
    public Vector3 viewDirection;
    public Vector3 viewPoint;
    public NavMeshAgent navMeshAgent;
    public float fireCooldown;
    public Transform spine;
    public float halfOfView;
    public LayerMask obstacleLayer;
    public float maxPatrolX;
    public float maxPatrolZ;
    private Vector3 startPosition;
    public float speedRotate;
    public float cosMinToSee = 0.98f;
    public float searchingTime;
    public float smoothRotate;
    public float maxRotateRange = 30;
    public bool HasGroup()
    {
        return group == null;
    }
    public bool IsGroupLeader()
    {
        return group.IsLeader(this);
    }
    public bool IsGroupFollower()
    {
        return IsGroupLeader() == false;
    }
    public void AnimCrossFade(int animation, float transition)
    {
        animator.CrossFade(animation, transition);
    }
    public void AnimInstant(int animation)
    {
        animator.Play(animation);
    }
    public bool IsCurrentAnimStop(int animation)
    {
        AnimatorStateInfo stateInfor = animator.GetCurrentAnimatorStateInfo(0);
        if(stateInfor.shortNameHash == animation && stateInfor.normalizedTime>=1.0){
            return true;
        }
        else
        {
            return false;
        }
    }
    public Vector3 GetStartPosition()
    {
        return startPosition;
    }
    private void Awake()
    {
        startPosition = transform.position;
    }
    private void Update()
    {
        direction = new Vector3(velocity.x, 0, velocity.z).normalized;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + transform.forward * maxPatrolZ / 2 - transform.right * maxPatrolX / 2, transform.position + transform.forward * maxPatrolZ / 2 + transform.right * maxPatrolX / 2);
        Gizmos.DrawLine(transform.position - transform.forward * maxPatrolZ / 2 - transform.right * maxPatrolX / 2, transform.position - transform.forward * maxPatrolZ / 2 + transform.right * maxPatrolX / 2);
        Gizmos.DrawLine(transform.position - transform.forward * maxPatrolZ / 2 - transform.right * maxPatrolX / 2, transform.position + transform.forward * maxPatrolZ / 2 - transform.right * maxPatrolX / 2);
        Gizmos.DrawLine(transform.position - transform.forward * maxPatrolZ / 2 + transform.right * maxPatrolX / 2, transform.position + transform.forward * maxPatrolZ / 2 + transform.right * maxPatrolX / 2);
    }
}
