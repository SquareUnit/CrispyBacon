using UnityEngine;

public class ControllerChargeState : IStates
{
    private RiderController user;
    private Vector3 currentVelocity;
    private float directionSmooth;
    private string stateName = "Controller Charge State";
    string IStates.StateName 
    { 
        get => stateName;
        set => stateName = value;
    }

    public ControllerChargeState(RiderController _userController)
    {
        user = _userController;
    }

    public void Enter()
    {
        user.DirectionVector = user.transform.forward;
        directionSmooth = 0.6f;
    }

    public void IfStateChange()
    {
        if (!user.inputSystem.IsCharging)
        {
            user.controllerStateMachine.ChangeState(user.defaultState);
        }
    }

    public void StateUpdate()
    {
        UpdateRotation();
        UpdateDirection();
        user.FillChargeMeter();
    }

    public void Exit()
    {
        user.RotationSpeed *= 0.18f;
    }

    private void UpdateRotation()
    {
        // Determin speed change
        if (user.inputSystem.IsMovementPerformed)
        {
            user.RotationSpeed += user.rotationAcceleration;

            if (user.inputSystem.SteeringDirectionReversed)
            {
                user.RotationSpeed *= 0.50f;
                user.inputSystem.SteeringDirectionReversed = false;
            }
        }
        else
        {
             user.RotationSpeed -= user.rotationAcceleration;
        }

        if (user.controllerIsCurrentlyMoving)
        {
            user.RotationSpeed = Mathf.Clamp(user.RotationSpeed, 0, user.rotationMaximumSpeed * 1f);
        }
        else
        {
            user.RotationSpeed = Mathf.Clamp(user.RotationSpeed, 0, user.rotationMaximumSpeed * 1.25f);
        }

        // Find rotation angle
        user.RotationTargetAngle = user.inputSystem.LeftStick.x * user.RotationSpeed;

        user.DesiredRotation = Quaternion.Euler(0, user.transform.eulerAngles.y + user.RotationTargetAngle, 0);
        user.CurrentRotation = Quaternion.Lerp(user.CurrentRotation, user.DesiredRotation, 0.11f);
        user.transform.rotation = user.CurrentRotation;
    }

    private void UpdateDirection()
    {
        // Set acceleration
        user.MovementSpeed -= user.movementAcceleration * 0.25f; //55f
        user.MovementSpeed = Mathf.Clamp(user.MovementSpeed, 0, user.movementMaximumSpeed);

        directionSmooth += 0.01f;
        user.Rb.velocity = Vector3.SmoothDamp(user.Rb.velocity, Vector3.Normalize(user.DirectionVector + user.transform.forward * 2000) * user.MovementSpeed, ref currentVelocity, directionSmooth);
        user.Rb.velocity += new Vector3(user.groundHit.normal.x, 0, user.groundHit.normal.z) * 5;
    }

    /// Notes on drift
    // Drift occurs when a forward force is kept (DirectionVector) while adding the controller forward * by a fucking lot (idky). 
    // The pivot point shift impression is given by smoothdamping the velocity coupled with a low speed decrease. (maybe reduce speed loss more when actively turning, causing by long drift?)
    // Making the smooth time bigger the longer the break seem to help, but fails to bring consistency in the movement (seem like too many variables in flux simultaneously)
}
