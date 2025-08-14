using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Windows;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerSkillManager))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    static public PlayerController Instance;

    [Header("Ref Component")]
    [SerializeField]
    private PlayerAnimationManager m_animationManager;
    [SerializeField]
    private CharacterController m_characterController;
    [SerializeField]
    private AudioSource m_audioSource;
    [SerializeField]
    private AudioClip[] m_footStepClips;

    [SerializeField]
    private PlayerInput m_playerInput;
    [SerializeField]
    private PlayerSkillManager m_playerSkillManager;
    [SerializeField]
    private PlayerUIManager m_playerUIManager;

    [Header("Movement Config")]
    public float MoveSpeed = 2;
    public float TurnSpeed = 20f;

    private Camera m_mainCamera;
    private Vector3 m_moveDir;
    
    private float m_rotationSmoothTime = 0.2f;
    private float m_currentSmoothVelocity = 0;

    private Collider[] colliders = new Collider[50];

    private float YAxis;
    private bool isLight = false;

    int m_num = 0;
    //private LayerMask m_wallLayerMask = LayerMask.GetMask("Wall");
    //private LayerMask m_enemyLayerMask = LayerMask.GetMask("Enemy");
    private void Awake()
    {
        Instance = this;

        m_characterController = GetComponent<CharacterController>();
        m_animationManager = GetComponentInChildren<PlayerAnimationManager>();
        m_playerSkillManager = GetComponent<PlayerSkillManager>();
        m_playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        m_mainCamera = Camera.main;
    }


    void Update()
    {
        HandleMove();
        HandleRotate();
        //Movement();
        //Rotate();

        //OnOffFlashLight();

        /*RaycastHit hit;
        if (Physics.Raycast(FlashLight.transform.position, FlashLight.transform.forward, out hit, FlashSkillDistance, WallLayerMask))
        {
            if (hit.collider != null)
            {
                m_skillDis = Vector3.Distance(FlashLight.transform.position, hit.transform.position);
            }
            else
            {
                m_skillDis = FlashSkillDistance;
            }
        }*/

        /*if (isLight)
        {
            FlashLightSkill();
        }*/
    }
    private void HandleMove()
    {
        float _targetSpeed = MoveSpeed;
        if (m_playerInput.MoveDir == Vector3.zero) _targetSpeed = 0f;
       
        // 카메라기준으로 캐릭터 이동
        Vector3 _forward = m_mainCamera.transform.forward;
        _forward.y = 0f;    // y값에 따라 높이가 변해버리기에 0으로 설정
        Vector3 _right = m_mainCamera.transform.right;

        // 캐릭터나 월드기준이 아닌 카메라 앞을 기준(기본 TPS 방식)
        m_moveDir = _forward * m_playerInput.MoveDir.z + _right * m_playerInput.MoveDir.x;
        
        m_characterController.Move(m_moveDir * _targetSpeed * Time.deltaTime);

        // Anime
        bool isMove = m_moveDir != Vector3.zero;
        m_animationManager.PlayWalkAnim(isMove);

        if(isMove)
        {
            if (!m_audioSource.isPlaying)
            {
                m_audioSource.Play();
            }
        }
    }

    private void HandleRotate()
    {
        if (m_playerInput.MoveDir == Vector3.zero) return;

        // 목표 회전 방향 (카메라 앞을 기준으로 왼쪽 오른쪽 키값으로의 회전 방향 추출)
        Quaternion targetRot = Quaternion.LookRotation(m_moveDir);
        // 목표 회전의 Y각도 추출
        float _targetAngle = targetRot.eulerAngles.y;

        // 카메라 추적이나 물리적 반응에는 SmoothDamp사용
        float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref m_currentSmoothVelocity, m_rotationSmoothTime);

        // 캐릭터 회전 적용
        transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);
    }

    /*void Rotate()
    {
        YAxis += Input.GetAxis("Mouse X") * TurnSpeed;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, YAxis, transform.eulerAngles.z);
    }*/

/*
    void OnOffFlashLight() 
    {
        FlashLight.intensity = LightFower;

        //���콺 ���� ������ �� �ҷ�����
        if (Input.GetMouseButtonDown(0))
        {
            if (!isLight && !m_isFlashCoolTime) //On
            {
                isLight = true;
                FlashLight.gameObject.SetActive(isLight);
                m_playerUIManager.SkillUIOn(SkillType.Flash);

            }
            else if (isLight) //Off
            {
                SetFlashOff();
            }
        }

        if (isLight && !m_isFlashCoolTime)
        {
            m_onFlashTime += Time.deltaTime;

            if (m_onFlashTime > FlashDuration)
            {
                SetFlashOff();
                m_onFlashTime = 0;
            }
        }
        if (m_isFlashCoolTime)
        {
            skillCoolTime += Time.deltaTime;
            //FlashIcon.fillAmount -= Time.smoothDeltaTime / FlashCoolTime;
            if (skillCoolTime > FlashCoolTime)
            {
                m_isFlashCoolTime = false;
                skillCoolTime = 0;
            }
        }
    }

    //�ߺ����
    void SetFlashOff()
    {
        isLight = false;
        FlashLight.gameObject.SetActive(isLight);
        //P
        m_isFlashCoolTime = true;
        //FlashIcon.fillAmount = 1;
    }

    /// <summary>
    /// 빛 조명을 이용한 스킬
    /// </summary>
    private void FlashLightSkill()
    {
        Vector3 conPos = FlashLight.transform.forward * m_skillDis;

        m_flashHalfAngle = SpotLightAngle / 2f;


        //중첩 유령들 확인
        int rangeofTargetNum = Physics.OverlapSphereNonAlloc(transform.position,m_skillDis,colliders, EnemyLayerMask);

        if (rangeofTargetNum == 0) return;

        for (int i = 0; i < rangeofTargetNum; i++)
        {
            Vector3 targetDir = (colliders[i].gameObject.transform.position - FlashLight.transform.position).normalized;

            float leftAngle = Vector3.Angle(transform.position, leftRayDir);
            float rightAngle = Vector3.Angle(transform.position, rightRayDir);
            float targetAngle = Vector3.Angle(transform.position, targetDir);

            if ((leftAngle <= targetAngle && rightAngle >= targetAngle) || (leftAngle >= targetAngle && rightAngle <= targetAngle))
            {
                IFlash target = colliders[i].gameObject.GetComponent<IFlash>();
                //플래시 당한 타겟에게 전달
                FlashMessage flashmessage;
                flashmessage.isFlash = true;

                target.ApplyFlash(flashmessage);
            }
        }
    }*/
}
