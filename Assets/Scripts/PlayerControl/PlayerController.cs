using System;
using Assets.Scripts.PlayerControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IHealth
{
    // Start is called before the first frame update
    
    public StateMachine movingStateMachine;
    private IdleState idleState;
    private WalkState walkState;
    private RunState runState;
    private OnAirState onAirState;


    private StateMachine armsStateMachine;
    private ArmStateNoneAction armStateNoneAction;
    private ArmStateLeftClick armStateLeftClick;
    private ArmSateRPress armSateRPress;


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
            EventCenter.Instance().OnHealthChange(currentHealth, maxHealth);
        } }
    public bool isGround;
    [SerializeField]
    public float force;
    [SerializeField]
    public float addForce;
    [SerializeField] public float groundDrag;
    [SerializeField] public float downForceOnAir;
    [SerializeField] public Vector3 jumpForce;
    [SerializeField] private Transform foot;
    [SerializeField] private float footRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] LayerMask itemLayer;
    [SerializeField] private float sensitivity;
    [SerializeField] private Transform rootTransform;
    [SerializeField] private Transform environmentCamera;
    private float horizontalRotation = 90;
    private float verticalRotation;
    private Animator playerAnimator;
    private float slowdownTime = 1.0f;
    private float slowdownTimer;
    private void Awake()
    {
        
    }

    void Start()
    {
        currentHealth = GameManager.Instance().saveData.currentHealth;
        playerAnimator = GetComponentInChildren<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        movingStateMachine = new StateMachine();
        armsStateMachine = new StateMachine();
        idleState = new IdleState(this);
        walkState = new WalkState(this);
        runState = new RunState(this);
        onAirState = new OnAirState(this);
        armStateNoneAction = new ArmStateNoneAction(this);
        armStateLeftClick = new ArmStateLeftClick(this);
        armSateRPress = new ArmSateRPress(this);
        movingStateMachine.currentState = idleState;
        armsStateMachine.currentState = armStateNoneAction;
        Cursor.lockState = CursorLockMode.Locked;
        EventCenter.Instance().saveAction += SavePlayer;
        EventCenter.Instance().onUsingItemLeftClick += ChangeStateToLeftClick;
        EventCenter.Instance().onUsingItemR += ChangeStateToRPress;
        EventCenter.Instance().onUsingNone += ChangeStateToNoneAction;
    }
    // Update is called once per frame
    void Update()
    {
        CheckGround();
        MovingStateHandle();
        movingStateMachine.UpdateCurrentState();
        armsStateMachine.UpdateCurrentState();
        LockUnlockCursor();
        HandleInteract();
    }

    private void FixedUpdate()
    {
        movingStateMachine.FixedUpdeateCurrentState();
        armsStateMachine.FixedUpdeateCurrentState();
    }
    private void LockUnlockCursor()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
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
    }
    private void MovingStateHandle()
    {
        if (isGround)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerRigidbody.AddForce(jumpForce, ForceMode.Impulse);
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.LeftShift) && slowdownTimer < 0)
                {
                    movingStateMachine.ChangeState(runState);
                }
                else
                {
                    movingStateMachine.ChangeState(walkState);
                }
            }
            else
            {
                movingStateMachine.ChangeState(idleState);
            }
            playerRigidbody.drag = groundDrag;
        }
        else
        {
            playerRigidbody.drag = 0;
            movingStateMachine.currentState = onAirState;
        }
        if (armsStateMachine.currentState == armStateNoneAction && slowdownTimer >= 0)
        {
            slowdownTimer -= Time.deltaTime;
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
    public void LoadPlayer()
    {
        currentHealth = GameManager.Instance().saveData.currentHealth;
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
        armsStateMachine.ChangeState(armStateLeftClick);
        slowdownTimer = slowdownTime;
    }
    public void ChangeStateToRPress()
    {
        armsStateMachine.ChangeState(armSateRPress);
        slowdownTimer = slowdownTime;
    }
    public void ChangeStateToNoneAction()
    {
        armsStateMachine.ChangeState(armStateNoneAction);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(foot.position, footRadius);
    }

    public void IncreaseHealth(int amount)
    {
        currentHealth += amount;
    }

    public void DecreaseHealth(int amount)
    {
        currentHealth -= amount;
    }
}