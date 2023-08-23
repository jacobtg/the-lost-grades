using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPCamera : MonoBehaviour
{
    private Vector3 _persistentCameraRot = new Vector3(20, 90, 0);
    
    [SerializeField] private float mouseSensitivity = 3.5f;
    [SerializeField] private float pitchMin = -90.0f;
    [SerializeField] private float pitchMax = 90.0f;

    public void LateUpdate()
    {
        if (!PlayerController.Instance.freeze)
        {
            float mousePitchInput = Input.GetAxisRaw("Mouse Y");
            float mouseYawInput = Input.GetAxisRaw("Mouse X");

            _persistentCameraRot.x -= mousePitchInput * mouseSensitivity * Time.deltaTime;
            _persistentCameraRot.y += mouseYawInput * mouseSensitivity * Time.deltaTime;

            _persistentCameraRot.x = Mathf.Clamp(_persistentCameraRot.x, pitchMin, pitchMax);

            transform.eulerAngles = _persistentCameraRot;            
        }
    }
}
