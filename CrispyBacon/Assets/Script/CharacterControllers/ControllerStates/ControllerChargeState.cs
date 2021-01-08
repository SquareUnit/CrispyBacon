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
        if (user.inputSystem.IsMovementPerformed)
        {
            user.RotationSpeed += user.rotationAcceleration;
            user.RotationSpeed = Mathf.Clamp(user.RotationSpeed, 0.0f, user.rotationMaximumSpeed);
            user.RotationTarget = user.inputSystem.Direction.x * user.RotationSpeed;
        }
        else
        {
            if (user.RotationTarget != 0.0f)
            {
                if (Mathf.Abs(user.RotationTarget) < 0.001f)
                {
                    user.RotationTarget = 0.0f;
                    user.RotationSpeed = 0.0f;
                }
                else
                {
                    user.RotationTarget *= user.rotationDragStrength;
                    user.RotationSpeed -= user.rotationAcceleration;
                    user.RotationSpeed = Mathf.Clamp(user.RotationSpeed, 0.0f, user.rotationMaximumSpeed);
                }
            }
        }

        user.DesiredRotation = Quaternion.Euler(0.0f, user.transform.eulerAngles.y + user.RotationTarget, 0.0f);
        user.CurrentRotation = Quaternion.Lerp(user.CurrentRotation, user.DesiredRotation, 0.11f);
        user.transform.rotation = user.CurrentRotation;
    }

    private void UpdateDirection()
    {
        user.MovementSpeed -= user.movementAcceleration / 2;
        user.MovementSpeed = Mathf.Clamp(user.MovementSpeed, 0.0f, user.movementMaximumSpeed);
        user.Rb.velocity = user.DirectionVector * user.MovementSpeed;
    }
}
