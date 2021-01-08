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

    void Awake()
    {
        controls = new InputMaster();
        controls.Player.Charge.performed += _ => Charge();
        controls.Player.AirBreaks.performed += _ => AirBreaks();
        controls.Player.Movement.performed += ctx => MovementPerformed(ctx.ReadValue<Vector2>());
        controls.Player.Movement.canceled += ctx => MovementCancelled();
    }

    private void MovementPerformed(Vector2 _direction)
    {
        if(inputsAreEnabled)
        {
            //Debug.Log("Movement Performed");
            direction = _direction;
            isMovementPerformed = true;
        }
    }

    private void MovementCancelled()
    {
        if (inputsAreEnabled)
        {
            //Debug.Log("Movement Cancelled");
            isMovementPerformed = false;
        }
    }

    private void Charge()
    {
        if (inputsAreEnabled)
        {
            Debug.Log("Charge performed");
        }
    }

    private void AirBreaks()
    {
        if (inputsAreEnabled)
        {
            Debug.Log("Airbreak performed");
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
