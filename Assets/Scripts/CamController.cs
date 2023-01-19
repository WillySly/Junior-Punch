using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CamController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] float sensitivity = 60f;


    CinemachineComponentBase componentBase;
    float maxFOV = 90f;
    float minFOV = 30f;

    // Update is called once per frame
    void Update()
    {
        HandleZoom();
        HandleLockUnlock();
    }

    private void HandleZoom()
    {
            float scrollWheelAction = Input.GetAxis("Mouse ScrollWheel");

            if (scrollWheelAction != 0)
            {
                float currentFOV = virtualCamera.m_Lens.FieldOfView;
                float targetFOV = currentFOV - scrollWheelAction*sensitivity;

                virtualCamera.m_Lens.FieldOfView = Mathf.Clamp(Mathf.Lerp(currentFOV, targetFOV, sensitivity * Time.deltaTime), minFOV, maxFOV);

        }
        
    }

    private void HandleLockUnlock()
    {
    }
}
