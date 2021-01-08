using UnityEngine;

public class CameraDefaultState : IStates
{
    private PlayerCamera userCamera;
    private Vector3 smoothVelocity;
    private float smoothTime;
    private string stateName = "Camera Default State";
    string IStates.StateName 
    { 
        get => stateName; 
        set => stateName = value; 
    }

    public CameraDefaultState(PlayerCamera _userCamera)
    {
        userCamera = _userCamera;
    }

    public void Enter()
    {
        smoothVelocity = Vector3.zero;
        smoothTime = userCamera.smoothTime;
    }

    public void IfStateChange()
    {/*
        if (InputsManager.instance.camButton)
        {
            userCamera.cameraStateMachine.ChangeState(userCamera.resetState);
        }
        
        else if (userCamera.isColliding)
        {
            userCamera.cameraStateMachine.ChangeState(userCamera.collisionState);
        }
        else if (GameManager.instance.currentAvatar.velocityY < 0)
        {
            userCamera.cameraStateMachine.ChangeState(userCamera.fallingState);
        }*/
    }

    public void StateUpdate()
    {
        userCamera.tr.position = Vector3.SmoothDamp(
            userCamera.tr.position, 
            userCamera.cameraTarget.tr.position - userCamera.tr.forward * userCamera.DesiredDollyDistance, 
            ref smoothVelocity, 
            smoothTime);
    }

    public void Exit()
    {

    }
}
