using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotYSpeed;
    GameObject camView;
    float XAxis;
    float YAxis;
    private void Awake()
    {

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        XAxis -= Input.GetAxis("Mouse Y") * rotYSpeed;
        XAxis = Mathf.Clamp(XAxis, -30, 60);
        PlayerController.Instance.camView.transform.eulerAngles = new Vector3(XAxis, PlayerController.Instance.camView.transform.eulerAngles.y, PlayerController.Instance.camView.transform.eulerAngles.z);
    }
}
