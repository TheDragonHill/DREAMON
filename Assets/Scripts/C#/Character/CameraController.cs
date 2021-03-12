using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;

    public float pitch = 2f;

    [SerializeField]
    float maxZoom = 10f;

    [SerializeField]
    float minZoom = 2f;

    [Range(0, 0.25f)]
    [SerializeField]
    float zoomSpeed = 0.05f;

    [SerializeField]
    float betweenFacesSpeed = 0.1f;


    float targetZoom = 0;
    float zoomValue = 0;

    private float currentZoom = 10f;
    private float currentYaw = 0;

    bool fixedCamera = false;
    bool onOffsetReset = false;
    public bool onLookAtLerp = false;

    public float drivingTime = 2;

    public float dampingTime = 15;
    float timer = 0;

    void Update()
    {
        if (!fixedCamera)
        {
            NotFixedCamera();
        }
    }

    #region UpdateMethods

    void NotFixedCamera()
    {
        //Rotates the camera when you hold down the middle mouse button
        RotateCameraOnInput();

        // Zooms the Camera
        Camerazoom();
    }

    void RotateCameraOnInput()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * Time.deltaTime * 1000f, Vector3.up);

            offset = camTurnAngle * offset;
        }
    }

    #endregion

    void LateUpdate()
    {
        if (!fixedCamera)
        {
            //The camera follows the player
            FollowPlayer();
            
        }
        else if(!onLookAtLerp)
        {
            LookAtPlayerNormal();
        }
        else
        {
            LookAtFacesInDialogue();
        }
    }

    #region LateUpdateMethods

    void FollowPlayer()
    {
        transform.position = target.position - offset * currentZoom;
        transform.LookAt(target.position + Vector3.up * pitch);

        transform.RotateAround(target.position, Vector3.up, currentYaw);
    }

    void LookAtPlayerNormal()
    {
        if (!onOffsetReset)
            LookAtTarget(target.position);
        else
            transform.LookAt(target.position + Vector3.up * pitch);
    }

    void LookAtFacesInDialogue()
    {
        transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(
                    target.position - transform.position),
                timer += Time.deltaTime * betweenFacesSpeed);
    }

    #endregion

    #region MoveCamera

    /// <summary>
    /// Move Camera to a fixed position, instead of following the player
    /// </summary>
    public void MoveToFixedPosition(Vector3 newPosition, Transform newTarget)
    {
        fixedCamera = true;
        target = newTarget;

        // Move Camera to Position
        transform.DOMove(newPosition, drivingTime);
    }

    /// <summary>
    /// Move Camera back to following player
    /// </summary>
    public void MoveToOffset(Transform newTarget)
    {
        onOffsetReset = true;
        target = newTarget;
        transform.DOMove(target.position - offset * currentZoom, drivingTime);
    }

    #endregion

    /// <summary>
    /// Trigger LookAtFaces() on moving between faces in dialogue
    /// </summary>
    public void LerpLookAt(Transform newLookAt)
    {
        onLookAtLerp = true;
        timer = 0;
        target = newLookAt;
        CancelInvoke(nameof(ResetLookAt));
        Invoke(nameof(ResetLookAt), drivingTime / 2);
    }

    #region ResetCamera

    public void StartResetCameraToPlayer()
    {
        Invoke(nameof(ResetCameraToPlayer), drivingTime);
    }

    public void CancelResetCameraToPlayer()
    {
        onOffsetReset = false;
        CancelInvoke(nameof(ResetCameraToPlayer));
    }
    void ResetLookAt()
    {
        onLookAtLerp = false;
    }
    void ResetCameraToPlayer()
    {
        onOffsetReset = false;
        fixedCamera = false;
    }

    #endregion

    /// <summary>
    /// Control Speed of LookAt
    /// </summary>
    /// <param name="focusPoint">Point to focus on</param>
    void LookAtTarget(Vector3 focusPoint)
    {
        var rotation = Quaternion.LookRotation(focusPoint- transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * dampingTime);
    }

    /// <summary>
    /// Zoom Camera on Mousewheel
    /// </summary>
    void Camerazoom()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            // Set End Value after Zoom
            targetZoom = currentZoom - Input.mouseScrollDelta.y;
        }
        else if (Mathf.Abs(targetZoom) > 0 && Mathf.Abs(targetZoom - currentZoom) > 0.0001f)
        {
            // Zoom for one part of the route
            if (Mathf.Abs(targetZoom - currentZoom) > 0.6f)
            {
                // Zoom in normal Speed
                if (targetZoom > maxZoom)
                    currentZoom = Mathf.SmoothDamp(currentZoom, maxZoom, ref zoomValue, zoomSpeed);
                else if (targetZoom < minZoom)
                    currentZoom = Mathf.SmoothDamp(currentZoom, minZoom, ref zoomValue, zoomSpeed);
                else
                    currentZoom = Mathf.SmoothDamp(currentZoom, targetZoom, ref zoomValue, zoomSpeed);
            }
            // Zoom slower for the last part of the route
            else if(Input.mouseScrollDelta.y == 0)
            {
                zoomValue = 0;

                currentZoom = Mathf.SmoothDamp(currentZoom, targetZoom, ref zoomValue, zoomSpeed * 2);
            }
        }
        else
        {
            // Reset zoom to be
            targetZoom = 0;
        }
    }

}