using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    public InputMaster controls;
    public bool debugIsEnabled = false;
    public bool inputsAreEnabled = true;
    private Vector2 direction;
    public Vector2 Direction { get => direction; }
    private bool isMovementPerformed;
    public bool IsMovementPerformed { get => isMovementPerformed; }
    private bool isCharging;
    public bool IsCharging { get => isCharging; }

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

    private void MovementPerformed(Vector2 _direction)
    {
        if(inputsAreEnabled)
        {
            //Debug.Log("Steering");
            direction = _direction;
            isMovementPerformed = true;
        }
    }

    private void MovementCancelled()
    {
        if (inputsAreEnabled)
        {
            //Debug.Log("Not steering");
            isMovementPerformed = false;
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

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
