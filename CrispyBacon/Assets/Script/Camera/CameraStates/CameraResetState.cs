using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This state reset the rider camera to a default position
public class CameraResetState : IStates
{
    private PlayerCamera userCamera;
    private Vector3 resetSV;
    public float timer;
    private float timerDefault = 0.65f;
    private float valueToDesiredPitch;
    private float valueToDesiredYaw;
    private string stateName = "Camera Reset State";
    string IStates.StateName
    {
        get => stateName;
        set => stateName = value;
    }

    public CameraResetState(PlayerCamera user)
    {
        userCamera = user;
    }

    public void Enter()
    {
        if (userCamera.debugMode) Debug.Log("sCamReset <color=yellow>Enter<color>");
        timer = timerDefault;
        //InputsManager.instance.cameraInputsAreDisabled = true;
    }

    public void IfStateChange()
    {

    }

    public void StateUpdate()
    {
        if (userCamera.debugMode) Debug.Log("sCamReset <color=blue>Update</color>");
        timer -= 1.0f * Time.deltaTime;
        if (timer <= 0)
        {
            if (userCamera.debugMode) Debug.Log("From sReset to sDefault <color=purple>StateChange</color>");
            userCamera.cameraStateMachine.ChangeState(userCamera.defaultState);
        }

        userCamera.pitch = Mathf.Lerp(userCamera.pitch, 15.0f, 0.10f);
        userCamera.yaw = Mathf.LerpAngle(userCamera.yaw, userCamera.cameraTarget.tr.rotation.eulerAngles.y, 0.10f);
        userCamera.tr.position = Vector3.SmoothDamp(userCamera.tr.position, userCamera.cameraTarget.tr.position - userCamera.tr.forward * userCamera.DesiredDollyDistance, ref resetSV, 0.025f);
    }

    public void Exit()
    {
        //InputsManager.instance.cameraInputsAreDisabled = false;
        if (userCamera.debugMode) Debug.Log("sCamReset <color=yellow>Exit<color>");
    }
}
