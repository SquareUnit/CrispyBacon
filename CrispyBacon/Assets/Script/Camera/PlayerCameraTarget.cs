using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraTarget : MonoBehaviour
{
    public Transform tr;
    public PlayerCamera userCamera;
    public float baseHeight = 2;
    public float verticalDolly;
    private RaycastHit sphereCastHit;
    private float sphereCastRadius = 0.4f;
    private float sphereCastDistance = 0.65f;

    public float heightAdjustment;
    private Vector3 forwardOffset;
    public float forwardOffsetValue = 0.35f;
    private Vector3 verticalOffset;

    private Vector3 currentVelocityRef;
    private float smoothMaximumSpeed = 25.0f;
    private float smoothTime = 0.06f;
    private Color debugColor;

    public Vector3 revealTargetPos;
    public Vector3 revealInitPos;
    public float revealLerpTime;
    public float t;
    public float tStamp;

    public void Start()
    {
        tr = GetComponent<Transform>();
        baseHeight = GetComponentInParent<RiderController>().height;
        tr.position = new Vector3(tr.position.x, tr.position.y + baseHeight, tr.position.z);
    }

    public void LateUpdate()
    {
        CalculateCameraTargetOffsets();
        SetTargetRotation();
        SetTargetPosition();
    }

    private void CalculateCameraTargetOffsets()
    {
        forwardOffset = tr.forward * forwardOffsetValue;
        verticalOffset.y = baseHeight + heightAdjustment + verticalDolly;
    }


    private void SetTargetRotation()
    {
        transform.rotation = userCamera.riderController.transform.rotation;
    }

    private void SetTargetPosition()
    {
        // If there is an object in front of avatar, do not offset camTarget forward or will will be on the other side of the wall
        if (Physics.SphereCast(userCamera.tr.position + verticalOffset, sphereCastRadius, tr.forward, out sphereCastHit, sphereCastDistance, userCamera.ObstaclesLayerMask))
        {
            tr.position = Vector3.SmoothDamp(tr.position, userCamera.riderController.transform.position + verticalOffset, ref currentVelocityRef, smoothTime, smoothMaximumSpeed);
            debugColor = Color.red;
        }
        else
        {
            tr.position = Vector3.SmoothDamp(tr.position, userCamera.riderController.transform.position + verticalOffset + forwardOffset, ref currentVelocityRef, smoothTime, smoothMaximumSpeed);
            debugColor = Color.green;
        }
    }

    private void OnDrawGizmos()
    {
        {
            Debug.DrawRay(tr.position, tr.forward * sphereCastDistance, debugColor);
            Gizmos.color = debugColor;
            if(sphereCastHit.point != new Vector3(0, 0, 0))
            {
                Gizmos.DrawWireSphere(sphereCastHit.point, sphereCastRadius);
            }
            Gizmos.DrawSphere(tr.position, 0.05f);
        }
    }

    public RaycastHit SphereCastHit
    {
        get { return sphereCastHit; }
    }
}


/*
private void SetCinematicPosition()
{

    if (!user.revealState.revealStartDone)
    {
        t = Mathf.SmoothStep(0.0f, 1.0f, (Time.time - tStamp) / revealLerpTime);
        transform.position = Vector3.Lerp(revealInitPos, revealTargetPos, t);
    }

    if (user.revealState.revealPauseDone)
    {
        t = Mathf.SmoothStep(0.0f, 1.0f, (Time.time - tStamp) / (revealLerpTime / 2));
        transform.position = Vector3.Lerp(revealTargetPos, animRootPos + yOffset + forwardOffset, t);
    }
    debugColor = Color.green;
}
*/