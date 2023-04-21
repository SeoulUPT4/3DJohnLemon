using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    static public PlayerController Instance;
    public float turnSpeed = 20f;
    public float moveSpeed;
    public float lightFower;
    public GameObject camView;
    public Light flashLight;
    public float rayLength;
    private float spotlightAngle;
    //private float halfAngle;

    private Vector3 lightPos;
    private float lightDir;


    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;

    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    float YAxis;
    bool isLight = false;

    private void Awake()
    {
        flashLight.gameObject.SetActive(false);
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this);
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
        spotlightAngle = flashLight.spotAngle;
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 moveDir = new Vector2(horizontal, vertical);
        if (moveDir.sqrMagnitude >= 1) moveDir = moveDir.normalized;
        Vector3 newDir = transform.forward * moveDir.y + transform.right * moveDir.x;
        transform.position = transform.position + newDir.normalized * Time.deltaTime * moveSpeed;
        //m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(newDir.x, 0f);
        bool hasVerticalInput = !Mathf.Approximately(newDir.y, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        } else
        {
            m_AudioSource.Stop();
        }

        YAxis += Input.GetAxis("Mouse X") * turnSpeed;
        transform.eulerAngles= new Vector3(transform.eulerAngles.x, YAxis, transform.eulerAngles.z);
        lightPos = flashLight.transform.position;
        float halfAngle = spotlightAngle * 0.5f;
        RaycastHit[] hits = Physics.RaycastAll(flashLight.transform.position, flashLight.transform.forward, rayLength);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];

            Vector3 targetDirection = hit.point - lightPos;
            float angle = Vector3.Angle(flashLight.transform.forward, targetDirection);

            if(angle <= halfAngle)
            {

            }
        }

        
    }
    private void LateUpdate()
    {
        if(Input.GetMouseButtonDown(0))
        {
            flashLight.intensity = lightFower;
            isLight = !isLight;
            flashLight.gameObject.SetActive(isLight);
        }
        Debug.DrawRay(flashLight.transform.position,flashLight.transform.forward * lightDir, Color.red);

    }

    void OnAnimatorMove()
    {
        //m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        //m_Rigidbody.MoveRotation(m_Rotation);
    }
}
