using System;
using Assets.Scripts.PlayerControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private IdleState idleState;
    private WalkState walkState;
    private RunState runState;
    private OnAirState onAirState;
    public PlayerStateMachine moveStateMachine;
    private ArmStateNone armStateNone;
    private ArmStateFire armStateFire;
    private ArmSateReload armSateReload;
    private PlayerStateMachine armsStateMachine;
    public Rigidbody playerRigidbody;
    [SerializeField] private int maxHealth;
    private int _currentHealth;
    [SerializeField] private int currentHealth { 
        get 
        { 
            return _currentHealth; 
        } 
        set {
            _currentHealth = Mathf.Clamp(value, 0, maxHealth);
            //PlayerInformation.Instance().OnHealthChange(currentHealth, maxHealth);
            EventCenter.Instance().OnHealthChange(currentHealth, maxHealth);
        } }
    public bool isGround;
    [SerializeField]
    private float gravityAcceleration;
    [SerializeField]
    public float speed;
    [SerializeField]
    public float addSpeed;
    [SerializeField]
    public Vector3 jumpForce;
    public Vector3 directionBeforeJump;
    [SerializeField]
    private Transform foot;
    [SerializeField]
    private float footRadius;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField] LayerMask itemLayer;
    [SerializeField]
    private float sensitivity;
    [SerializeField] private Transform rootTransform;
    [SerializeField] private Transform environmentCamera;
    private float horizontalRotation = 90;
    private float verticalRotation;
    private Animator playerAnimator;
    [SerializeField]
    private GameObject holdGun;
    private float delayTime = 1.0f;
    private float delayTimer;
    public bool limitY;
    private void Awake()
    {
        
    }

    void Start()
    {
        currentHealth = GameManager.Instance().saveData.currentHealth;
        playerAnimator = GetComponentInChildren<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        moveStateMachine = new PlayerStateMachine();
        armsStateMachine = new PlayerStateMachine();
        idleState = new IdleState(this);
        walkState = new WalkState(this);
        runState = new RunState(this);
        onAirState = new OnAirState(this);
        armStateNone = new ArmStateNone(this);
        armStateFire = new ArmStateFire(this);
        armSateReload = new ArmSateReload(this);
        moveStateMachine.currentState = idleState;
        armsStateMachine.currentState = armStateNone;
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance().saveAction += SavePlayer;
        //SelectionBar.Instance().onUsingItemLeftClick += ChangeStateToLeftClick;
        //SelectionBar.Instance().onUsingItemR += ChangeStateToR;
        //SelectionBar.Instance().onUsingNone += ChangeStateToNone;
        EventCenter.Instance().onUsingItemLeftClick += ChangeStateToLeftClick;
        EventCenter.Instance().onUsingItemR += ChangeStateToR;
        EventCenter.Instance().onUsingNone += ChangeStateToNone;
    }
    // Update is called once per frame
    void Update()
    {
        CheckGround();
        StateHandle();
        moveStateMachine.UpdateCurrentState();
        armsStateMachine.UpdateCurrentState();
        if(Cursor.lockState == CursorLockMode.Locked)
        {
            ItemInputHandle();
            RotateView();
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        HandleInteract();
    }

    private void FixedUpdate()
    {
        moveStateMachine.FixedUpdeateCurrentState();

    }
    private void StateHandle()
    {
        if (isGround)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                limitY = false;
                playerRigidbody.AddForce(jumpForce, ForceMode.VelocityChange);
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                    if (Input.GetKey(KeyCode.LeftShift) && delayTimer < 0)
                    {
                        moveStateMachine.ChangeState(runState);
                    }
                    else
                    {
                        moveStateMachine.ChangeState(walkState);
                    }
                    
            }
            else
            {
                    moveStateMachine.ChangeState(idleState);
            }
        }
        else
        {
            moveStateMachine.currentState = onAirState;
        }
        if(armsStateMachine.currentState == armStateNone && delayTimer >= 0)
        {
            delayTimer -= Time.deltaTime;
        }
    }
   
    private void ItemInputHandle()
    {
        if (Input.GetMouseButton(0))
        {
            SelectionBar.Instance().PlayerLeftClick();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            SelectionBar.Instance().PlayerPressG();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SelectionBar.Instance().PlayerPressR();
        }
    }
    private void CheckGround()
    {
        isGround = Physics.CheckSphere(foot.position, footRadius, groundLayer);
       
    }

    private void RotateView()
    {
        horizontalRotation += Input.GetAxis("Mouse X") * sensitivity;
        verticalRotation -= Input.GetAxis("Mouse Y") * sensitivity;
        horizontalRotation = Mathf.Repeat(horizontalRotation, 360);
        verticalRotation = Mathf.Clamp(verticalRotation, -80, 80);
        transform.localRotation = Quaternion.Euler(0, horizontalRotation, 0);
        rootTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
    public void ChangeAnimation(int animation, float crossFade)
    {
        playerAnimator.CrossFade(animation, crossFade, -1, 0);
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
    void SavePlayer()
    {
        GameManager.Instance().saveData.currentHealth = currentHealth;
    }
    void HandleInteract()
    {
        RaycastHit hit;
        Physics.Raycast(environmentCamera.position, environmentCamera.forward, out hit, 2, itemLayer);
        IInteractable interact = hit.collider?.GetComponent<IInteractable>();
        if(interact != null)
        {
            if (Input.GetKeyDown(KeyCode.F)) interact.Interact();
            UIWorldSpace.Instance().ShowIcon(hit.collider.transform.position);
        }
        else
        {
            UIWorldSpace.Instance().HideIcon();
        }
    }
    public void ChangeStateToLeftClick()
    {
        armsStateMachine.ChangeState(armStateFire);
        delayTimer = delayTime;
    }
    public void ChangeStateToR()
    {
        armsStateMachine.ChangeState(armSateReload);
        delayTimer = delayTime;
    }
    public void ChangeStateToNone()
    {
        armsStateMachine.ChangeState(armStateNone);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(foot.position, footRadius);
    }
}