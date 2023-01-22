using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CamController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera followingCamera;
    [SerializeField] CinemachineVirtualCamera roamingCamera;

    [SerializeField] float sensitivity = 60f;
    [SerializeField] float panSpeed = 20f;

    bool cameraLocked = true;
    Transform roamingCameraTransform;
    List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();
    CinemachineVirtualCamera activeCamera;

    float maxFOV = 90f;
    float minFOV = 30f;

    private void Awake()
    {
        roamingCameraTransform = roamingCamera.VirtualCameraGameObject.transform;

        // Not the optimal way if we have more cameras, but will work for now
        cameras.Add(followingCamera);
        cameras.Add(roamingCamera);
        SwitchCamera(followingCamera);
    }

    private void Update()
    {
        HandleZoom();
        HandleLockUnlock();
    }

    private void HandleZoom()
    {
            float scrollWheelAction = Input.GetAxis("Mouse ScrollWheel");

            if (scrollWheelAction != 0)
            {
                float currentFOV = activeCamera.m_Lens.FieldOfView;
                float targetFOV = currentFOV - scrollWheelAction*sensitivity;

                activeCamera.m_Lens.FieldOfView = Mathf.Clamp(Mathf.Lerp(currentFOV, targetFOV, Time.deltaTime), minFOV, maxFOV);
        }
        
    }

    private Vector3 PanDirection(float x, float z)
    {
        Vector3 direction = Vector3.zero;

        if (z >= Screen.height * .95f)
        {
            direction.z += 1;
        }
        else if (z <= Screen.height * .05f)
        {
            direction.z -= 1;
        }

        if (x >= Screen.width * .95f)
        {
            direction.x += 1;
        }
        else if (x <= Screen.width * .05f)
        {
            direction.x -= 1;
        }

        return direction;
    }

    private void PanScreen(float x, float z)
    {
        Vector3 direction = PanDirection(x, z);
        roamingCameraTransform.position = Vector3.Lerp(roamingCameraTransform.position, roamingCameraTransform.position + direction * panSpeed, Time.deltaTime);

    }

    private bool IsActiveCamera(CinemachineVirtualCamera camera)
    {
        return camera == activeCamera;
    }

    private void SwitchCamera(CinemachineVirtualCamera camera)
    {
        camera.Priority = 2;
        activeCamera = camera;

        foreach (CinemachineVirtualCamera c in cameras)
        {
            if (c != camera)
                c.Priority = 0;
        }


    }

    private void AdjustRoamingCameraPosition()
    {
        roamingCameraTransform.position = followingCamera.VirtualCameraGameObject.transform.position;
        roamingCameraTransform.rotation = followingCamera.VirtualCameraGameObject.transform.rotation;
    }

    private void HandleLockUnlock()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            cameraLocked = !cameraLocked;
            if (cameraLocked)
            {
                SwitchCamera(followingCamera);
            }
            else {
                SwitchCamera(roamingCamera);
                AdjustRoamingCameraPosition();
            }
        }

        if (!cameraLocked)
        {
            PanScreen(Input.mousePosition.x, Input.mousePosition.y);
        }
    }
}
