using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderController : MonoBehaviour
{
    public InputSystem inputSystem;
    public PlayerCamera playerCamera;
    public PlayerCamera playerCameraRef;
    private Rigidbody rb;
    public bool useRiderUI;
    public RiderInfoUI riderUI;

    public Rigidbody Rb { get => rb; set => rb = value; }
    [Tooltip("The height of the camera target, relative to root GO origin")]
    public float height = 1;
    [Tooltip("Is the controller currently moving?")]
    public bool controllerIsCurrentlyMoving;

    // State Machine
    public StateMachine controllerStateMachine;
    public ControllerDefaultState defaultState;
    public ControllerChargeState chargeState;

    // Rotation
    private Quaternion desiredRotation;
    private Quaternion currentRotation;
    private float rotationTargetAngle;
    private float rotationSpeed;

    public Quaternion DesiredRotation { get => desiredRotation; set => desiredRotation = value; }
    public Quaternion CurrentRotation { get => currentRotation; set => currentRotation = value; }
    public float RotationTargetAngle { get => rotationTargetAngle; set => rotationTargetAngle = value; }
    public float RotationSpeed { get => rotationSpeed; set => rotationSpeed = value; }

    [Tooltip("How fast the craft attains it's maximum turn amplitude")]
    public float rotationAcceleration = 0.5f;
    [Tooltip("How sharp the craft can turn")]
    public float rotationMaximumSpeed = 12.0f;
    //[Tooltip("How fast the craft naturally lose turn momentum")] 
    //public float rotationDragStrength = 0.975f;

    // Position
    private Vector3 directionVector;
    private float movementSpeed;

    public Vector3 DirectionVector { get => directionVector; set => directionVector = value; }
    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }

    [Tooltip("How fast the craft gain forward speed")]
    public float movementAcceleration = 0.6f;
    [Tooltip("The maximum speed the craft can reach by simple acceleration")]
    public float movementMaximumSpeed = 18.5f;

    // Charge
    public float chargeMeter;
    public float chargeMeterMaximumCapacity = 0.8f;
    public float chargeMeterFillRate = 1;
    public float chargeMeterDepletionRate = 5;

    // Other
    [Tooltip("The ride is considered immobile below this magnitude threshold.\n It's an adjustment to fit the player perception.")]
    public float perceivedImmobilityPoint = 2.75f; // Will come in conflict with slopes for rides like the slick star that should forever slide.

    //Stats
    public float weight;
    public float turn;
    public float charge;
    public float boost;
    public float fly;
    public float topSpeed;
    public float offenceDefence;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputSystem = GetComponent<InputSystem>();
        playerCamera = Instantiate(playerCameraRef, transform.position, transform.rotation, transform);

        controllerStateMachine = GetComponent<StateMachine>();
        defaultState = new ControllerDefaultState(this);
        chargeState = new ControllerChargeState(this);

        controllerStateMachine.ChangeState(defaultState);

        if(useRiderUI)
        {
            riderUI = Instantiate(riderUI, new Vector3(0, 0, 0), Quaternion.identity);
            riderUI.riderController = this;
        }
    }

    void FixedUpdate()
    {
        CheckIfControllerInMovement();

        controllerStateMachine.CheckIfStateChange();
        controllerStateMachine.CurrentStateUpdate();

        

    }

    private void CheckIfControllerInMovement()
    {
        Debug.Log(rb.velocity.magnitude);
        if (rb.velocity.magnitude > perceivedImmobilityPoint)
        {
            controllerIsCurrentlyMoving = true;
        }
        else
        {
            controllerIsCurrentlyMoving = false;
            rb.velocity = new Vector3(0, 0, 0);
        }
    }

    public void FillChargeMeter()
    {
        chargeMeter += Time.deltaTime * chargeMeterFillRate;
        chargeMeter = Mathf.Clamp(chargeMeter, 0, chargeMeterMaximumCapacity);
    }

    public void DepleteChargeMeter()
    {
        chargeMeter -= Time.deltaTime * chargeMeterDepletionRate;
        chargeMeter = Mathf.Clamp(chargeMeter, 0, chargeMeterMaximumCapacity);
    }
}
