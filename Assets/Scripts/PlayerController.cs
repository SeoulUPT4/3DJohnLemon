using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    static public PlayerController Instance;

    [Header("플레이어 설정")]
    public float turnSpeed = 20f;
    public float moveSpeed;
    public Image flashOffImage;
    public Image flashSkillCoolTimeImage;
    [Header("카메라 설정")]
    [Tooltip("카메라 Target은 레몬의 자식오브젝트에 있음")]
    public GameObject camView;

    [Header("레이어 마스크 설정")]
    public LayerMask wallLayerMask;
    public LayerMask enemyLayerMask;
    
    [Header("손전등관련 설정")]
    public Light flashLight;
    public float spotLightAngle;
    public float lightFower;
    public float flashRange; //실제 빛추는 거리
    public float flashSkillDistance; //스킬 사거리
    public float skillRadius; //검출 반경

    [Header("스킬관련 설정")]
    public float onFlashT;
    public float skillCoolT;

    private float onFlashTime;
    private bool isFlashCoolTime;
    private float skillCoolTime;

    List<GameObject> detectedObjects = new List<GameObject>();
    float m_skillDis;
    float m_flashHalfAngle;
    Vector3 m_SkillPos;
    Vector3 leftRayDir;
    Vector3 rightRayDir;

    RaycastHit[] hits = new RaycastHit[50];
    Collider[] colliders = new Collider[50];

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;

    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    float YAxis;
    bool isLight = false;
    Material originMaterial;
    Rigidbody rigid;

    private void Awake()
    {
        Instance = this;

        rigid = GetComponent<Rigidbody>();

        flashLight.gameObject.SetActive(false);
    }

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
        flashLight.range = flashRange;
        flashLight.spotAngle = spotLightAngle;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        flashOffImage.gameObject.SetActive(true);
        flashSkillCoolTimeImage.fillAmount = 0;

    }
    private void FixedUpdate()
    {
        //왼쪽과 오른쪽 스킬 방향과 사거리
        leftRayDir = Quaternion.Euler(0f, -m_flashHalfAngle, 0f) * transform.forward * m_skillDis;
        rightRayDir = Quaternion.Euler(0f, m_flashHalfAngle, 0f) * transform.forward * m_skillDis;

    }
    void Update()
    {
        Movement();
        Rotate();

        OnOffFlashLight();

        RaycastHit hit;
        if (Physics.Raycast(flashLight.transform.position, flashLight.transform.forward, out hit, flashSkillDistance, wallLayerMask))
        {
            if (hit.collider != null)
            {
                m_skillDis = Vector3.Distance(flashLight.transform.position, hit.transform.position);
            }
            else
            {
                m_skillDis = flashSkillDistance;
            }
        }

        if (isLight)
        {
            FlashLightSkill();
        }
    }

    void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 moveDir = new Vector2(horizontal, vertical);
        if (moveDir.sqrMagnitude >= 1) moveDir = moveDir.normalized;

        Vector3 newDir = transform.forward * moveDir.y + transform.right * moveDir.x;

        rigid.velocity = newDir.normalized * moveSpeed;

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
        }
        else
        {
            m_AudioSource.Stop();
        }
    }

    void Rotate()
    {
        YAxis += Input.GetAxis("Mouse X") * turnSpeed;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, YAxis, transform.eulerAngles.z);
    }

    //손전등 켜고 끄기 + 쿨타임
    void OnOffFlashLight() 
    {
        flashLight.intensity = lightFower;

        //마우스 왼쪽 눌렀을 때 불러오기
        if (Input.GetMouseButtonDown(0))
        {
            if (!isLight && !isFlashCoolTime) //On
            {
                isLight = true;
                flashLight.gameObject.SetActive(isLight);
                flashOffImage.gameObject.SetActive(false);

            }
            else if (isLight) //Off
            {
                SetFlashOff();
            }
        }

        if (isLight && !isFlashCoolTime)
        {
            onFlashTime += Time.deltaTime;

            if (onFlashTime > onFlashT)
            {
                SetFlashOff();
                onFlashTime = 0;
            }
        }
        if (isFlashCoolTime)
        {
            skillCoolTime += Time.deltaTime;
            flashSkillCoolTimeImage.fillAmount -= Time.smoothDeltaTime / skillCoolT;
            if (skillCoolTime > skillCoolT)
            {
                isFlashCoolTime = false;
                skillCoolTime = 0;
            }
        }
    }

    //중복사용
    void SetFlashOff()
    {
        isLight = false;
        flashLight.gameObject.SetActive(isLight);
        flashOffImage.gameObject.SetActive(true);
        isFlashCoolTime = true;
        flashSkillCoolTimeImage.fillAmount = 1;
    }

    //손전등 스킬
    void FlashLightSkill()
    {
        Vector3 conPos = flashLight.transform.forward * m_skillDis;

        m_flashHalfAngle = spotLightAngle / 2f;


        //현재 플레이어로부터 스킬 반경까지의 오브젝트들 모두 추출
        int rangeofTargetNum = Physics.OverlapSphereNonAlloc(transform.position,m_skillDis,colliders, enemyLayerMask);

        if (rangeofTargetNum == 0) return;

        for (int i = 0; i < rangeofTargetNum; i++)
        {
            Vector3 targetDir = (colliders[i].gameObject.transform.position - flashLight.transform.position).normalized;

            float leftAngle = Vector3.Angle(transform.position, leftRayDir);
            float rightAngle = Vector3.Angle(transform.position, rightRayDir);
            float targetAngle = Vector3.Angle(transform.position, targetDir);

            if ((leftAngle <= targetAngle && rightAngle >= targetAngle) || (leftAngle >= targetAngle && rightAngle <= targetAngle))
            {
                IFlash target = colliders[i].gameObject.GetComponent<IFlash>();
                //적에게 스킬 사용 시 적 중지
                FlashMessage flashmessage;
                flashmessage.isFlash = true;

                target.ApplyFlash(flashmessage);
            }
        }
    }
}
