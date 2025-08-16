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

    private bool m_isFlashLight = false;
    private bool m_isMapScan = false;

    private bool m_isSkill = false;

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
        HandleSkill();

        if (m_isSkill) return;
        HandleMove();
        HandleRotate();
    }

    #region ================================================================================ Movement
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

        // Move Anime
        bool isMove = m_moveDir != Vector3.zero;
        m_animationManager.PlayWalkAnime(isMove);

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
    #endregion ================================================================================ /Movement

    #region ================================================================================ Skill
    private void HandleSkill()
    {
        if(!m_isFlashLight && !m_isSkill)
        {
            if (m_playerInput.IsFlash)
            {
                m_isFlashLight = true;
                m_isSkill = true;
                // Skill
                m_playerSkillManager.OnFlashLight();

                
                // Skill Anime
                m_animationManager.PlayFlashAnime(m_playerSkillManager.FlashDuration);
                
                // Skill CoolTimeUI
                StartCoroutine(SkillCoolTime(SkillType.Flash, m_playerSkillManager.FlashCoolTime));

                // IsSkill 복구
                Invoke("InvokeIsSkill", m_playerSkillManager.FlashDuration);
            }
        }
        if (!m_isMapScan && !m_isSkill)
        {
            if (m_playerInput.IsMapScan)
            {
                m_isMapScan = true;
                m_isSkill = true;
                // Skill

                // Skill Anime
                m_animationManager.PlayMapScanAnime(m_playerSkillManager.MapSacnDuration);

                // Skill CoolTimeUI
                StartCoroutine(SkillCoolTime(SkillType.MapScan, m_playerSkillManager.MapScanCoolTime));

                // IsSkill 복구
                Invoke("InvokeIsSkill", m_playerSkillManager.MapSacnDuration);
            }
        }
    }

    private void InvokeIsSkill()
    {
        m_isSkill = false;
    }

    private IEnumerator SkillCoolTime(SkillType skillType, float CoolTime)
    {
        m_playerUIManager.ActivateCoolTimeUI(skillType, true);

        float duration = 0;
        while (duration < 1)
        {
            duration += Time.deltaTime/ CoolTime;
            m_playerUIManager.ChargingCoolTimeUI(skillType, 1-duration);
            yield return null;
        }

        m_playerUIManager.ActivateCoolTimeUI(skillType, false);
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
}
