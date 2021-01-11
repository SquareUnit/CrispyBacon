using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    public InputMaster controls;
    public bool debugIsEnabled = false;
    public bool inputsAreEnabled = true;

    private Vector2 lastLeftStickValue = new Vector2(0, 0);

    private Vector2 leftstick;
    private bool isMovementPerformed;
    private bool isCharging;
    private bool steeringDirectionReversed = false;

    public Vector2 LeftStick { get => leftstick; }
    public bool IsMovementPerformed { get => isMovementPerformed; }
    public bool IsCharging { get => isCharging; }
    public bool SteeringDirectionReversed 
    { 
        get => steeringDirectionReversed; 
        set => steeringDirectionReversed = value; 
    }

    void Awake()
    {
        controls = new InputMaster();
        controls.Player.Charge.started += _ => ChargeStarted();
        controls.Player.Charge.performed += _ => ChargePerformed();
        controls.Player.Charge.canceled += _ => ChargeCanceled();
        controls.Player.AirBreaks.performed += _ => AirBreaks();
        controls.Player.Movement.performed += ctx => MovementPerformed(ctx.ReadValue<Vector2>());
        controls.Player.Movement.canceled += ctx => MovementCancelled();
    }

    private void Update()
    {

    }

    private void MovementPerformed(Vector2 _leftStick)
    {
        if(inputsAreEnabled)
        {
            //Debug.Log("Steering");
            leftstick = _leftStick;
            CheckForSteeringDirectionChange();
            //if (steeringDirectionHasChanged) Debug.Log("Steering Direction Changed");
            lastLeftStickValue = _leftStick;
            isMovementPerformed = true;
        }
    }

    private void MovementCancelled()
    {
        if (inputsAreEnabled)
        {
            //Debug.Log("Not steering");
            isMovementPerformed = false;
            leftstick.x = 0;
        }
    }
    private void ChargeStarted()
    {
        if (inputsAreEnabled)
        {
            //Debug.Log("Charging Initiating");
            isCharging = true;
        }
    }

    private void ChargePerformed()
    {
        if (inputsAreEnabled)
        {
            //Debug.Log("Charging");
        }
    }

    private void ChargeCanceled()
    {
        if (inputsAreEnabled)
        {
            //Debug.Log("Charging Canceled");
            isCharging = false;
        }
    }

    private void AirBreaks()
    {
        if (inputsAreEnabled)
        {
            //Debug.Log("Airbreak performed");
        }
    }

    /// <summary> Check if the player is suddently changing the direction he's turning</summary>
    private void CheckForSteeringDirectionChange()
    {
        //Debug.Log($"Last left stick value: {lastLeftStickValue.x} \n Direction is: {direction.x}");
        if (lastLeftStickValue.x > 0 && leftstick.x <= 0 ||
            lastLeftStickValue.x < 0 && leftstick.x >= 0)
        {
            steeringDirectionReversed = true;
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
