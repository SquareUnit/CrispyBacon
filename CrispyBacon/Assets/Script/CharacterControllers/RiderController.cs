using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderController : MonoBehaviour
{
    public InputSystem inputSystem;
    public PlayerCamera playerCamera;
    public PlayerCamera playerCameraRef;
    private Rigidbody rb;
    [Tooltip("The height of the camera target, relative to root GO origin")]
    public float height = 1;
    [Tooltip("Is the controller currently moving?")]
    public bool controllerInMovement;

    // Rotation
    private Quaternion desiredRotation;
    private Quaternion currentRotation;
    private float rotationTarget;
    [Tooltip("How fast the craft attains it's maximum turn amplitude")]
    public float rotationAcceleration = 0.5f;
    private float rotationSpeed;
    [Tooltip("How sharp the craft can turn")]
    public float rotationMaximumSpeed = 12.0f;
    [Tooltip("How fast the craft naturally lose turn momentum")]
    public float rotationDragStrength = 0.975f;

    // Position
    [Tooltip("How fast the craft gain forward speed")]
    public float movementAcceleration = 0.25f;
    private float movementSpeed;
    [Tooltip("The maximum speed the craft can reach")]
    public float movementMaximumSpeed = 20.0f;

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
    }

    void FixedUpdate()
    {
        UpdateRotation();
        UpdatePosition();

        if (rb.velocity != new Vector3(0, 0, 0))
        {
            controllerInMovement = true;
        }
        else
        {
            controllerInMovement = false;
        }
    }

    private void UpdateRotation()
    {
        if (inputSystem.IsMovementPerformed)
        {
            rotationSpeed += rotationAcceleration;
            rotationSpeed = Mathf.Clamp(rotationSpeed, 0.0f, rotationMaximumSpeed);
            Debug.Log(rotationSpeed);
            rotationTarget = inputSystem.Direction.x * rotationSpeed;
        }
        else
        {
            if (rotationTarget != 0.0f)
            {
                if (Mathf.Abs(rotationTarget) < 0.001f)
                {
                    rotationTarget = 0.0f;
                    rotationSpeed = 0.0f;
                    Debug.Log(rotationSpeed);
                }
                else
                {
                    rotationTarget *= rotationDragStrength;
                    rotationSpeed -= rotationAcceleration;
                    rotationSpeed = Mathf.Clamp(rotationSpeed, 0.0f, rotationMaximumSpeed);
                    Debug.Log(rotationSpeed);
                }
            }
        }

        desiredRotation = Quaternion.Euler(0.0f, transform.eulerAngles.y + rotationTarget, 0.0f);
        currentRotation = Quaternion.Lerp(currentRotation, desiredRotation, 0.11f);
        transform.rotation = currentRotation;
    }

    private void UpdatePosition()
    {
        movementSpeed += movementAcceleration;
        movementSpeed = Mathf.Clamp(movementSpeed, 0.0f, movementMaximumSpeed);
        rb.velocity = transform.forward * movementSpeed;
    }
}
