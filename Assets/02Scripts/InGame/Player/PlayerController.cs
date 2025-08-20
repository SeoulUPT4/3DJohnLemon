using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerSkillManager))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    static public PlayerController Instance;

    [Header("Ref Component")]
    [SerializeField]
    private CharacterController m_characterController;
    [SerializeField]
    private AudioSource m_audioSource;
    [SerializeField]
    private AudioClip m_cryingClips;
    [SerializeField]
    private PlayerAnimationManager m_animationManager;
    [SerializeField]
    private PlayerInput m_playerInput;
    [SerializeField]
    private PlayerSkillManager m_playerSkillManager;

    [Header("Movement Config")]
    [SerializeField] private float m_moveSpeed = 2;
    [SerializeField] private float m_turnSpeed = 20f;

    private Camera m_mainCamera;
    private Vector3 m_moveDir;
    
    private float m_rotationSmoothTime = 0.1f;
    private float m_currentSmoothVelocity = 0;

    private bool m_isFlashLight = false;
    private bool m_isMapScan = false;

    private bool m_isSkill = false;
    private bool m_isDead = false;
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
        if(m_isDead) return;
        HandleSkill();

        HandleRotate();
        if (m_isSkill) return;
        HandleMove();
    }

    #region ================================================================================ Movement
    private void HandleMove()
    {
        float _targetSpeed = m_moveSpeed;
        if (m_playerInput.MoveDir == Vector3.zero) _targetSpeed = 0f;
       
        // 카메라기준으로 캐릭터 이동
        Vector3 _forward = m_mainCamera.transform.forward;
        _forward.y = 0f;    // 0f : y값(축)에 따라 카메라 바라보는 방향이 아래나 위 방향이면 캐릭터의 전진 방향이 지면이나 천장방향이되기에
        Vector3 _right = m_mainCamera.transform.right;

        // 캐릭터나 월드기준이 아닌 카메라 앞을 기준(기본 TPS 방식)
        m_moveDir = _forward * m_playerInput.MoveDir.z + _right * m_playerInput.MoveDir.x;
        m_characterController.Move(m_moveDir * _targetSpeed * Time.deltaTime);

        // Move Anime
        bool isMove = m_moveDir != Vector3.zero;
        m_animationManager.PlayWalkAni(isMove);

        // FootStep Audio
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
        // 목표 회전 방향 (카메라 앞을 기준으로 왼쪽 오른쪽 키값으로의 회전 방향 추출)
        Quaternion targetRot = Quaternion.identity;
        if (m_isSkill)
        {
            targetRot = Quaternion.LookRotation(m_mainCamera.transform.forward);
        }
        else
        {
            if (m_playerInput.MoveDir == Vector3.zero) return;
                targetRot = Quaternion.LookRotation(m_moveDir);
        }
        // 목표 회전의 Y각도 추출
        float _targetAngle = targetRot.eulerAngles.y;

        // 카메라 추적이나 물리적 반응에는 SmoothDamp사용
        float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref m_currentSmoothVelocity, m_rotationSmoothTime);

        // 캐릭터 회전 적용
        transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);
    }
    #endregion ================================================================================ /Movement

    #region ================================================================================ Skill
    private void HandleSkill()
    {
        if (m_isSkill) return;

        if (!m_isFlashLight && m_playerInput.IsFlash)
        {
            m_isFlashLight = true;
            m_isSkill = true;
            // Skill
            m_playerSkillManager.OnFlashLight();

            // Skill Anime
            float _time = m_playerSkillManager.FlashDuration;
            m_animationManager.PlayFlashAni(_time);

            // Skill CoolTimeUI
            StartCoroutine(SkillCoolTime(SkillType.Flash, m_playerSkillManager.FlashCoolTime));

            // IsSkill 복구
            Invoke("InvokeIsSkill", m_playerSkillManager.FlashDuration + 1);    // +1 다른 스킬 바로 사용 방지
        }
        else if (!m_isMapScan && m_playerInput.IsMapScan)
        {
            m_isMapScan = true;
            m_isSkill = true;
            // Skill
            m_playerSkillManager.OnMapScan(m_playerSkillManager.MapSacnDuration);

            // Skill Anime
            m_animationManager.PlayMapScanAni(m_playerSkillManager.MapSacnDuration);

            // Skill CoolTimeUI
            StartCoroutine(SkillCoolTime(SkillType.MapScan, m_playerSkillManager.MapScanCoolTime));

            // IsSkill 복구
            Invoke("InvokeIsSkill", m_playerSkillManager.MapSacnDuration + 2);  // +2 카메라 돌아오는 시간포함
        }
    }

    private void InvokeIsSkill()
    {
        m_isSkill = false;
    }

    private IEnumerator SkillCoolTime(SkillType skillType, float CoolTime)
    {
        UIManager.Instance.ActivateCoolTimeUI(skillType, true);

        float duration = 0;
        while (duration < 1)
        {
            duration += (Time.deltaTime / CoolTime);
            UIManager.Instance.ChargingCoolTimeUI(skillType, 1 - duration); // fillAmount는 1이 Full인 상태이므로 반대로 -시켜야함
            yield return null;
        }

        UIManager.Instance.ActivateCoolTimeUI(skillType, false);
        switch (skillType)
        {
            case SkillType.Flash:
                m_isFlashLight = false;
                break;
            case SkillType.MapScan:
                m_isMapScan = false;
                break;
        }
    }
    #endregion ================================================================================ /Skill

    private void OnTriggerEnter(Collider other)
    {
        // Enemy
        if(other.CompareTag("DeadZone"))
        {
            // GameOver ReStart처리
            GameManager.Instance.PlayerDead();
            m_isDead = true;

            // Anime
            m_animationManager.PlayDeadAni();
            
            // Audio
            m_audioSource.clip = m_cryingClips;
            m_audioSource.Play();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Interaction"))
        {
            if(m_playerInput.IsInteraction)
            {
                
            }
        }
    }

    
}
