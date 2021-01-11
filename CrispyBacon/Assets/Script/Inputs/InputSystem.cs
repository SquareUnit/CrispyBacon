using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    public InputMaster controls;
    public bool debugIsEnabled = false;
    public bool inputsAreEnabled = true;

    private Vector2 previousLeftStickValue = new Vector2(0, 0);

    private Vector2 leftStickValue;
    private Vector2 targetLeftStickValue;
    private Vector2 leftStickRef;
    private bool playerCurrentlySteering;
    private bool isCharging;
    private bool steeringDirectionReversed = false;

    public Vector2 LeftStick { get => leftStickValue; }
    public bool IsMovementPerformed { get => playerCurrentlySteering; }
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
        UpdateSteeringInfo();
        leftStickValue = Vector2.SmoothDamp(leftStickValue, targetLeftStickValue, ref leftStickRef, 0.04f);
        //Debug.Log($"Target: {targetLeftStickValue} Value: {leftStickValue}");
    }

    private void MovementPerformed(Vector2 _leftStick)
    {
        if(inputsAreEnabled)
        {
            targetLeftStickValue = _leftStick;
        }
    }

    private void MovementCancelled()
    {
        if (inputsAreEnabled)
        {
            playerCurrentlySteering = false;
            leftStickValue = new Vector2(0, 0);
            targetLeftStickValue = new Vector2(0, 0);
        }
    }
    private void ChargeStarted()
    {
        if (inputsAreEnabled)
        {
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

    private void UpdateSteeringInfo()
    {
        // Validate if player is currently steering
        if (targetLeftStickValue.x > 0 || targetLeftStickValue.x < 0)
        {
            playerCurrentlySteering = true;
        }
        else
        {
            return;
        }
        
        // Validate if player has drastically reversed direction
        if (previousLeftStickValue.x > 0 && targetLeftStickValue.x < 0 ||
            previousLeftStickValue.x < 0 && targetLeftStickValue.x > 0)
        {
            steeringDirectionReversed = true;
        }
        previousLeftStickValue = targetLeftStickValue;
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
