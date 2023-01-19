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
    CinemachineVirtualCamera activeCamera;
    List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();

    float maxFOV = 90f;
    float minFOV = 30f;


    private void Awake()
    {
        roamingCameraTransform = roamingCamera.VirtualCameraGameObject.transform;

        // Not the optimal way if we have more cameras, but will work this time
        cameras.Add(followingCamera);
        cameras.Add(roamingCamera);
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
        Debug.Log("Screen height: " + Screen.height + " current mouse z: " + z);
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
        Debug.Log("in PanScreen");

        Vector3 direction = PanDirection(x, z);

        Debug.Log("Direction is: " + direction);

        Vector3 initpos = roamingCameraTransform.position;
        roamingCameraTransform.position = Vector3.Lerp(roamingCameraTransform.position, roamingCameraTransform.position + direction * panSpeed, Time.deltaTime);
        //Debug.Log("camera transfor from: " + initpos + " to " + roamingCameraTransform.position);

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
