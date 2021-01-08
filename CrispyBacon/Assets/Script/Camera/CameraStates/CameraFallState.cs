using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFallingState : IStates
{
    private PlayerCamera userCamera;
    private RaycastHit hit;
    private Vector3 dollySV;

    private string stateName = "Camera Fall State";
    string IStates.StateName
    {
        get => stateName;
        set => stateName = value;
    }

    public CameraFallingState(PlayerCamera user)
    {
        userCamera = user;
    }

    public void Enter()
    {
        dollySV = Vector3.zero;
    }

    public void IfStateChange()
    {
        if (userCamera.isColliding)
        {
            userCamera.cameraStateMachine.ChangeState(userCamera.collisionState);
        }
        /*else if (userCamera.avatarFSM.currentState != userCamera.avatarFSM.fall)
        {
            userCamera.camFSM.ChangeState(userCamera.defaultState);
        }*/
    }

    public void StateUpdate()
    {
        RaisePitchIfObstacle();
        userCamera.tr.position = Vector3.SmoothDamp(userCamera.tr.position, userCamera.cameraTarget.tr.position - userCamera.tr.forward * userCamera.DesiredDollyDistance, ref dollySV, 0.025f);
    }

    public void Exit()
    {

    }

    /// <summary> While falling, check if something directly below might obstrude the camera. If so, attempt to move the cam pitch up preventively. 
    /// This is all made to try to avoid a transition to the collision state while falling, if & when possible</summary>
    private void RaisePitchIfObstacle()
    {
        Vector3 camDownStart = userCamera.tr.position - (1.6f * userCamera.tr.up);
        Vector3 camDownEnd = userCamera.cameraTarget.tr.position - userCamera.tr.position;
        
        if (Physics.Raycast(camDownStart, camDownEnd, out hit, userCamera.DesiredDollyDistance, userCamera.ObstaclesLayerMask) && hit.collider.tag != "AllowCameraDissolve")
        {
            Debug.DrawRay(camDownStart, camDownEnd, Color.red);
            userCamera.pitch += 200f * Time.deltaTime;
        } 
        else Debug.DrawRay(camDownStart, camDownEnd, Color.green);
    }
}



