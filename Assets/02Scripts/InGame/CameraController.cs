using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject CamView;
    public float RotationSpeed;
    
    private float m_cameraDirX;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        m_cameraDirX -= Input.GetAxis("Mouse Y");
        m_cameraDirX = Mathf.Clamp(m_cameraDirX, -30, 60);

        //Vector2 _cameraRotDir = new Vector3(m_cameraDirX, CamView.transform.eulerAngles.y, CamView.transform.eulerAngles.z);
        Vector2 _cameraRotDir = new Vector3(m_cameraDirX, 0, 0);
        CamView.transform.rotation = Quaternion.Euler(_cameraRotDir * RotationSpeed);
    }
}
