using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotYSpeed;
    GameObject camView;
    float XAxis;
    float YAxis;
    LayerMask layerMask = LayerMask.GetMask("Player");
    List<GameObject> prevAlphWalls = new List<GameObject>();
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
        //RayToPlayer();
    }

    private void LateUpdate()
    {
        
    }

    void RayToPlayer()
    {
        Vector3 pos = PlayerController.Instance.gameObject.transform.position;
        Vector3 dir = new Vector3(pos.x, pos.y + 0.5f, pos.z) - transform.position;
        float dis = Vector3.Distance(transform.position, pos);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, dir, dis, ~layerMask);
        HitColliders(hits);
    }

    void HitColliders(RaycastHit[] _hits)
    {
        if (_hits.Length != 0)
        {
            for (int i = 0; i < _hits.Length; i++)
            {

            }

        }
        


    }

}
