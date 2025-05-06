using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DronesController : MonoBehaviour
{
    [SerializeField] GameObject drone;
    List<DroneScript> listDrones;
    List<Vector3> listCoordinate;
    [SerializeField] float height;
    [SerializeField] Transform playerTransform;
    private NavMeshAgent navMeshAgent;
    public bool playerDetected=false;
    [SerializeField] private float timeToUpdateTarget;
    private float updateTargetTimer;
    private Vector3 lastKnowPosition;
    [SerializeField] float rangeToUpdatePosition;
    [Header("Patrol information")]
    private Vector3 rootPosition;
    [SerializeField] private float xPatrolArea;
    [SerializeField] private float zPatrolArea;
    private Vector3 destinationPatrol;
    private bool isGameStarted;
    [SerializeField] float timeChangePatrol;
    float changePatrolTimer;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = PlayerInformation.Instance().GetTransform();
        isGameStarted = true;
        rootPosition = transform.position;
        updateTargetTimer = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();
        listDrones = new List<DroneScript>();
        listCoordinate = new List<Vector3>();
        for (int i = 0; i < 1; i++)
        {
            Vector3 clonePosition = transform.position + Vector3.up * height;
            GameObject droneClone = Instantiate( drone,clonePosition,Quaternion.identity);
            DroneScript droneScript = droneClone.GetComponent<DroneScript>();
            droneScript.dronesController = this;
            droneScript.playerTransform = playerTransform;
            listDrones.Add(droneScript);
            listCoordinate.Add(clonePosition-transform.position);
        }
        changePatrolTimer = timeChangePatrol;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRootForDrone();
        if (playerDetected)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }
    void UpdateRootForDrone()
    {
        for(int i = 0; i < 1; i++)
        {
            Vector3 worldPosition = transform.TransformPoint(listCoordinate[i]);
            listDrones[i].setRootXZ(new Vector2(worldPosition.x, worldPosition.z));
            listDrones[i].setRootDirection(transform.forward);
        }
    }
    void ChasePlayer()
    {
        updateTargetTimer -= Time.deltaTime;
        if (updateTargetTimer <= 0 && (playerTransform.position-lastKnowPosition).magnitude>rangeToUpdatePosition)
        {
            navMeshAgent.destination = playerTransform.position;
            lastKnowPosition = playerTransform.position;
            updateTargetTimer = timeToUpdateTarget;
        }
    }
    void Patrol()
    {
        changePatrolTimer -= Time.deltaTime;
        if (destinationPatrol==Vector3.zero || (transform.position-destinationPatrol).magnitude<=5f || changePatrolTimer <=0)
        {
            Debug.Log("generate patrol position");
            destinationPatrol.x = rootPosition.x + Random.RandomRange(-xPatrolArea, xPatrolArea);
            destinationPatrol.z = rootPosition.z + Random.RandomRange(-zPatrolArea, zPatrolArea);
            destinationPatrol.y = rootPosition.y;
            NavMeshPath path = new NavMeshPath();
            if (navMeshAgent.CalculatePath(destinationPatrol, path))
            {
                navMeshAgent.SetPath(path);
            }
            changePatrolTimer = timeChangePatrol;
        }
    }
    private void OnDrawGizmos()
    {
        
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

    }
}
