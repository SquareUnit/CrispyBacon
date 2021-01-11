using UnityEngine;

public class ControllerDefaultState : IStates
{
    private RiderController user;
    private Vector3 currentVelocity; // Ref for the smooth damp function
    private string stateName = "Controller Default State";
    string IStates.StateName 
    { 
        get => stateName;
        set => stateName = value;
    }

    public ControllerDefaultState(RiderController _userController)
    {
        user = _userController;
    }

    public void Enter() {}

    public void IfStateChange()
    {
        if(user.inputSystem.IsCharging) user.controllerStateMachine.ChangeState(user.chargeState);
    }

    public void StateUpdate()
    {
        UpdateRotation();
        UpdateDirection();
        user.DepleteChargeMeter();
    }

    public void Exit() {}

    private void UpdateRotation()
    {
        // Determin speed change
        if (user.inputSystem.IsMovementPerformed)
        {
            user.RotationSpeed += user.rotationAcceleration;

            if (user.inputSystem.SteeringDirectionReversed)
            {
                user.RotationSpeed *= 0.18f;
                user.inputSystem.SteeringDirectionReversed = false;
            }
        }
        else
        {
            user.RotationSpeed -= user.rotationAcceleration;
        }
        user.RotationSpeed = Mathf.Clamp(user.RotationSpeed, 0, user.rotationMaximumSpeed);

        // Find rotation angle
        user.RotationTargetAngle = user.inputSystem.LeftStick.x * user.RotationSpeed;

        // Apply rotation
        user.DesiredRotation = Quaternion.Euler(0.0f, user.transform.eulerAngles.y + user.RotationTargetAngle, 0);
        user.CurrentRotation = Quaternion.Lerp(user.CurrentRotation, user.DesiredRotation, 0.11f);
        user.transform.rotation = user.CurrentRotation;
    }

    private void UpdateDirection()
    {
        // Determin movement speed
        if(user.chargeMeter != 0)
        {
            user.MovementSpeed += user.movementAcceleration * (7 * (user.chargeMeter / user.chargeMeterMaximumCapacity));
        }
        else
        {
            user.MovementSpeed += user.movementAcceleration;
        }
        
        if(user.MovementSpeed > user.movementMaximumSpeed)
        {
            user.MovementSpeed -= user.movementAcceleration * 1.2f;
        }

        // Set movement
        user.Rb.velocity = Vector3.SmoothDamp(user.Rb.velocity, user.transform.forward * user.MovementSpeed, ref currentVelocity, 0.01f);
    }
}
