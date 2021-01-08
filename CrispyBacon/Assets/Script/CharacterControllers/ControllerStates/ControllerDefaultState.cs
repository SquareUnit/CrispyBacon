using UnityEngine;

public class ControllerDefaultState : IStates
{
    private RiderController user;
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

    public void Enter()
    {

    }

    public void IfStateChange()
    {
        if(user.inputSystem.IsCharging)
        {
            user.controllerStateMachine.ChangeState(user.chargeState);
        }
    }

    public void StateUpdate()
    {
        if (user.inputSystem.IsMovementPerformed)
        {
            user.RotationSpeed += user.rotationAcceleration;
            user.RotationSpeed = Mathf.Clamp(user.RotationSpeed, 0.0f, user.rotationMaximumSpeed);
            //Debug.Log(user.RotationSpeed);
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
                    //Debug.Log(user.RotationSpeed);
                }
                else
                {
                    user.RotationTarget *= user.rotationDragStrength;
                    user.RotationSpeed -= user.rotationAcceleration;
                    user.RotationSpeed = Mathf.Clamp(user.RotationSpeed, 0.0f, user.rotationMaximumSpeed);
                    //Debug.Log(user.RotationSpeed);
                }
            }
        }

        user.DesiredRotation = Quaternion.Euler(0.0f, user.transform.eulerAngles.y + user.RotationTarget, 0.0f);
        user.CurrentRotation = Quaternion.Lerp(user.CurrentRotation, user.DesiredRotation, 0.11f);
        user.transform.rotation = user.CurrentRotation;

        // Set direction
        user.MovementSpeed += user.movementAcceleration;
        user.MovementSpeed = Mathf.Clamp(user.MovementSpeed, 0.0f, user.movementMaximumSpeed);
        user.DirectionVector = user.transform.forward;
        user.Rb.velocity = user.DirectionVector * user.MovementSpeed;
    }

    public void Exit()
    {

    }
}
