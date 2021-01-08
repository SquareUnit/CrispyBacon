using UnityEngine;

public class CameraCollisionState : IStates
{
    private PlayerCamera userCamera;
    private float hitPointDist;
    private Vector3 collSV;
    private float SmoothMaxSpeed = 25.0f;
    private string stateName = "Camera Collision State";
    string IStates.StateName
    {
        get => stateName;
        set => stateName = value;
    }

    public CameraCollisionState(PlayerCamera user)
    {
        userCamera = user;
    }

    public void Enter()
    {
        collSV = Vector3.zero;
    }

    public void IfStateChange()
    {/*
        if (!user.isColliding)
        {
            user.camFSM.ChangeState(user.defaultState);
        }
        else if (InputsManager.instance.camButton)
        {
            user.camFSM.ChangeState(user.resetState);
        }*/
    }

    public void StateUpdate()
    {
        Vector3 dirToTarget = userCamera.cameraTarget.tr.position - userCamera.hit.point;
        dirToTarget.y = 0.0f;
        float hitAngle = 90 - Vector3.Angle(userCamera.hit.normal, dirToTarget);
        float hyp = (userCamera.desiredCollisionOffset - 0.01f) / Mathf.Sin(Mathf.Deg2Rad * hitAngle); // Must be smaller than the camera desired collision offset because of float imprecision.
        hitPointDist = Vector3.Distance(userCamera.hit.point, userCamera.cameraTarget.tr.position);

        userCamera.tr.position = Vector3.SmoothDamp(userCamera.tr.position, userCamera.cameraTarget.tr.position - userCamera.tr.forward * (hitPointDist - hyp), ref collSV, 0.032f, SmoothMaxSpeed);
    }

    public void Exit()
    {

    }
}
