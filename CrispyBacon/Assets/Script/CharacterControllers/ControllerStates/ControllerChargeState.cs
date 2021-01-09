using UnityEngine;

public class ControllerChargeState : IStates
{
    private RiderController user;
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
        // Remember forward vector, create the sliding effect
        user.DirectionVector = user.transform.forward;
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
    }

    public void Exit()
    {
        user.MovementSpeed = user.movementMaximumSpeed;
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
                Debug.Log("<color=red>Direction Reversed</color>");
            }
        }
        else
        {
             user.RotationSpeed -= user.rotationAcceleration;
        }

        if (user.Rb.velocity.x < 0.01f && user.Rb.velocity.y < 0.01f && user.Rb.velocity.z < 0.01f)
        {
            user.RotationSpeed = Mathf.Clamp(user.RotationSpeed, 0, user.rotationMaximumSpeed * 2.5f);
        }
        else
        {
            user.RotationSpeed = Mathf.Clamp(user.RotationSpeed, 0, user.rotationMaximumSpeed);
        }

        // Find rotation angle
        user.RotationTargetAngle = user.inputSystem.LeftStick.x * user.RotationSpeed;

        Debug.Log(user.RotationSpeed);
        user.DesiredRotation = Quaternion.Euler(0, user.transform.eulerAngles.y + user.RotationTargetAngle, 0);
        user.CurrentRotation = Quaternion.Lerp(user.CurrentRotation, user.DesiredRotation, 0.11f);
        user.transform.rotation = user.CurrentRotation;
    }

    private void UpdateDirection()
    {
        // Set acceleration
        user.MovementSpeed -= user.movementAcceleration * 0.55f;
        user.MovementSpeed = Mathf.Clamp(user.MovementSpeed, 0, user.movementMaximumSpeed);
        user.Rb.velocity = user.DirectionVector * user.MovementSpeed;
    }
}
