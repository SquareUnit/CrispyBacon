using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputSystem : MonoBehaviour
{
    public InputMaster controls;
    private Vector2 direction;

    public Vector2 Direction { get => direction; }

    void Awake()
    {
        controls = new InputMaster();
        controls.Player.Charge.performed += _ => Charge();
        controls.Player.AirBreaks.performed += _ => AirBreaks();

        controls.Player.Movement.performed += ctx => direction = ctx.ReadValue<Vector2>();
        controls.Player.Movement.canceled += _ => direction = Vector2.zero;
    }

    private void Update()
    {
        //Debug.Log(Direction);
    }

    private void Charge()
    {
        // Slow down craft swiftly
        Debug.Log("Charge performed");
    }

    private void AirBreaks()
    {
        // Slow down a little and improve control over the craft
        Debug.Log("Airbreak performed");
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
