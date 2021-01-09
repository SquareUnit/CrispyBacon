using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerCamera : MonoBehaviour
{
    public Transform tr;
    public InputSystem inputSystem;
    public RiderController riderController;
    public new Camera camera;
    public PlayerCameraTarget cameraTargetRef;
    public PlayerCameraTarget cameraTarget;
    public List<string> obstacleLayerList = new List<string>();
    private LayerMask obstaclesLayerMask;
    public LayerMask ObstaclesLayerMask
    {
        get { return obstaclesLayerMask; }
    }

    private bool loadingDone = false;

    public StateMachine cameraStateMachine;
    public CameraDefaultState defaultState;
    public CameraCollisionState collisionState;
    public CameraFallingState fallingState;
    public CameraResetState resetState;


    public float cameraFieldOfView = 60.0f;
    public float yaw, pitch;
    public float smoothTime = 1.000f; // previously 0.022f, was supper stiff for unkown reason
    private float YawSpeedDirectControl { get; } = 126.0f;
    private float YawSpeedIndirectControl { get; } = 68.0f;
    private float PitchSpeed { get; } = 110.0f;

    public float pitchMinimumAngle = 10.0f;
    public float pitchMaximumAngle = 30.0f;

    private Quaternion currRotation { get; set; }
    private Quaternion desiredRotation { get; set; }
    private protected float a1, b1, a2, b2;

    // Camera dolly params
    private float desiredDollyDistance;
    [HideInInspector] public float DesiredDollyDistance { get => desiredDollyDistance; }
    public float camDollyMinDist = 2.4f;
    public float camDollyMaxDist = 5.0f;

    // Camera height Offset params
    private float desiredVerticalOffset;
    private float cameraTargetMinimumVerticalOffset = 0.0f;
    private float cameraTargetMaximumVerticalOffset = 0.15f;

    // Collision params
    private Vector3 directionToCameraTarget;
    private Collider lastCollFound;
    [HideInInspector] public RaycastHit hit;
    public float desiredCollisionOffset = 0.5f;
    public bool isColliding;

    // Sideray's params
    private RaycastHit hitRight, hitLeft;

    // Other
    public bool debugMode;
    public bool raycastsDebug = true;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        tr = GetComponent<Transform>();
        inputSystem = GetComponentInParent<InputSystem>();
        riderController = GetComponentInParent<RiderController>();
        camera = GetComponent<Camera>();
        camera.fieldOfView = cameraFieldOfView;

        SetCamDollyParams();

        cameraStateMachine = GetComponent<StateMachine>();
        defaultState = new CameraDefaultState(this);
        collisionState = new CameraCollisionState(this);
        fallingState = new CameraFallingState(this);
        resetState = new CameraResetState(this);

        StartCoroutine(WaitForLoading(0.0f));
    }

    //Change this for a callback
    private IEnumerator WaitForLoading(float delay)
    {
        yield return new WaitForSeconds(delay);
        cameraTarget = Instantiate(cameraTargetRef, tr.position, tr.rotation, tr);
        cameraTarget.userCamera = this;
        cameraStateMachine.ChangeState(defaultState);
        yield return new WaitForEndOfFrame();
        loadingDone = true;
    }
    private void FixedUpdate()
    {
        if (loadingDone)
        {
            UpdateCameraProperties();
            cameraStateMachine.CheckIfStateChange();
            cameraStateMachine.CurrentStateUpdate();
        }
    }

    private void UpdateCameraProperties()
    {
        SetYawAndPitch();
        CameraTargetVerticalDolly();
        CameraOrientation();
        CameraForwardBackwardDolly();
        //IsCameraColliding();
        CameraSideRays(3.0f, 1.2f);
    }

    private void SetYawAndPitch()
    {
        pitch = 15;
        pitch = Mathf.Clamp(pitch, pitchMinimumAngle, pitchMaximumAngle);
    }

    /// <summary> Displace the cam target upward or downward depending on the camera pitch. Linear function start dollying below 0 </summary>
    private void CameraTargetVerticalDolly()
    {
        desiredVerticalOffset = a2 * pitch + b2;
        cameraTarget.verticalDolly = Mathf.Clamp(desiredVerticalOffset, cameraTargetMinimumVerticalOffset, cameraTargetMaximumVerticalOffset);
    }

    ///<summary> Set up the camera orientation </summary>
    private void CameraOrientation()
    {
        yaw = cameraTarget.transform.eulerAngles.y;
        desiredRotation = Quaternion.Euler(pitch, yaw, 0.0f);
        currRotation = Quaternion.Lerp(currRotation, desiredRotation, 1f);
        tr.rotation = currRotation;
    }

    /// <summary> Dolly the camera forward or backward depending on the pitch. Linear function start dollying below 0 </summary>
    private void CameraForwardBackwardDolly()
    {
        desiredDollyDistance = a1 * pitch + b1;
        desiredDollyDistance = Mathf.Clamp(desiredDollyDistance, camDollyMinDist, camDollyMaxDist);
    }

    private void IsCameraColliding()
    {
        directionToCameraTarget = tr.position - cameraTarget.tr.position;
        if (raycastsDebug) Debug.DrawRay(cameraTarget.tr.position, directionToCameraTarget, Color.gray);

        //if (Physics.SphereCast(camTarget.tr.position, 0.10f,  dirToCamera, out hit, camDollyMaxDist, obstaclesLayerMask)) TODO: Integrate sphere cast
        if (Physics.Raycast(cameraTarget.tr.position, directionToCameraTarget, out hit, camDollyMaxDist * 2, obstaclesLayerMask))
        {
            float product = Vector3.Dot(hit.normal, Vector3.up);
            if (product <= 0.3 && product >= -0.3) // TODO : Handle ceilings(|| hit.normal.y < 0))
            {
                CheckCollisionValidity();
            }
            else isColliding = false;
        }
        else // If raycast not picking up anything
        {
            isColliding = false;
        }
    }

    /// <summary> Check if the collision distance is small enough for it to be considered valid for camera wall hoovering behaviour</summary>
    public void CheckCollisionValidity()
    {
        Vector3 hitToCamTarget = cameraTarget.tr.position - hit.point;
        hitToCamTarget.y = 0.0f;
        float hitAngle = Vector3.Angle(hit.normal, hitToCamTarget);
        float hitToCamDist = Vector3.Distance(hit.point, cameraTarget.tr.position) - desiredDollyDistance;
        float distanceFromCollisionToCamera = Mathf.Sin(Mathf.Deg2Rad * hitAngle) * hitToCamDist;

        if (distanceFromCollisionToCamera < desiredCollisionOffset) isColliding = true;
        else isColliding = false;
    }

    /// <summary> Cast rays parallel to the camera forward vector. Nudge the camera yaw in the opposite direction if one or multiple collides. </summary>
    private void CameraSideRays(float sideRayCount, float rayOffset)
    {
        if (riderController.controllerInMovement && !isColliding)
        {
            Vector3 rayOrigin;
            Vector3 rayDir;
            float rayLength;
            float speedCorrection = 35;

            for (int i = 1; i <= sideRayCount; i++)
            {
                // Rays position and yaw adjustment strength
                float raysLateralOffsets = Mathf.Abs(inputSystem.LeftStick.magnitude) * rayOffset * i / sideRayCount;
                float yawAdjustment = (1 + Mathf.Abs(inputSystem.LeftStick.magnitude)) * speedCorrection * i / sideRayCount; // Recheck line algo

                // Right parallel ray(s)
                rayOrigin = transform.position + (raysLateralOffsets * transform.right);
                rayDir = cameraTarget.transform.position - transform.position - (transform.forward * i / sideRayCount);
                rayLength = rayDir.magnitude;

                if (Physics.Raycast(rayOrigin, rayDir, out hitRight, rayLength, obstaclesLayerMask))
                {
                    if (raycastsDebug) Debug.DrawRay(rayOrigin, rayDir, Color.red);
                    yaw += yawAdjustment * Time.deltaTime;
                }
                else if (raycastsDebug) Debug.DrawRay(rayOrigin, rayDir, Color.green);

                // Left parallel ray(s) 
                rayOrigin = transform.position - (raysLateralOffsets * transform.right); // Try to make uniform with other rays
                rayDir = cameraTarget.transform.position - transform.position - (transform.forward * i / sideRayCount);
                rayLength = rayDir.magnitude;

                if (Physics.Raycast(rayOrigin, rayDir, out hitLeft, rayLength, obstaclesLayerMask))
                {
                    if (raycastsDebug) Debug.DrawRay(rayOrigin, rayDir, Color.red);
                    yaw -= yawAdjustment * Time.deltaTime;
                }
                else if (raycastsDebug) Debug.DrawRay(rayOrigin, rayDir, Color.green);
            }
        }
    }

    /// <summary> Determin how the forward/backward dolly and vertical offset will move the camera, depending on the parameters </summary>
    private void SetCamDollyParams()
    {
        // Forward/backward dolly
        a1 = (camDollyMinDist - camDollyMaxDist) / pitchMinimumAngle;
        b1 = camDollyMaxDist;

        // Upward/Downward setup
        a2 = (cameraTargetMaximumVerticalOffset - cameraTargetMinimumVerticalOffset) / pitchMinimumAngle;
        b2 = cameraTargetMinimumVerticalOffset;
    }
}

// Tool used to detect the nature of what the camera target is colliding with in front of the avatar.
#if UNITY_EDITOR
[CustomEditor(typeof(PlayerCamera))]
public class PlayerCameraDebugTool : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        PlayerCamera user = (PlayerCamera)target;

        if (GUILayout.Button("Get collision name"))
        {
            Debug.Log("Object " + user.cameraTarget.SphereCastHit.collider.transform.root.name + " is root of collision");
            if (user.cameraTarget.SphereCastHit.collider.transform.parent != null)
            {
                Debug.Log(user.cameraTarget.SphereCastHit.collider.transform.parent.name + " : is direct parent");
            }
            Debug.Log("Hitting the layer number " + user.cameraTarget.SphereCastHit.collider.gameObject.layer.ToString());
        }
    }
}
#endif

//Very good system to set up cam yaw and pitch before the states can determin how to lerp toward those values.
//Not in use with the current project need, integrate to a bigger system.
/*
private void SetYawAndPitch()
{
    // If camera stick not in use, use yaw for cam movements. Else, use camera stick.
    if (InputsManager.instance.rightStick.x > 0)
    {
        yaw += InputsManager.instance.rightStick.x * YawSpeedDirectControl * Time.deltaTime;
    }
    if (InputsManager.instance.rightStick.x < 0)
    {
        yaw -= Mathf.Abs(InputsManager.instance.rightStick.x) * YawSpeedDirectControl * Time.deltaTime;
    }

    if (InputsManager.instance.rightStick.x == 0 && InputsManager.instance.leftStick.x >= 0.25)
    {
        yaw += InputsManager.instance.leftStick.x * YawSpeedIndirectControl * Time.deltaTime;
    }
    if (InputsManager.instance.rightStick.x == 0 && InputsManager.instance.leftStick.x <= -0.25)
    {
        yaw -= Mathf.Abs(InputsManager.instance.leftStick.x) * YawSpeedIndirectControl * Time.deltaTime;
    }


    if (yaw > 360.0f)
    {
        yaw -= 360.0f;
        currRotation = new Vector3(currRotation.x, currRotation.y - 360, currRotation.z);
    }
    if (yaw < 0.0f)
    {
        yaw += 360.0f;
        currRotation = new Vector3(currRotation.x, currRotation.y + 360, currRotation.z);
    }

    // Setup pitch and clamp it
    pitch -= InputsManager.instance.rightStick.y * PitchSpeed * Time.deltaTime;
    pitch = Mathf.Clamp(pitch, pitchMinimumAngle, pitchMaximumAngle);
}
*/