using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEditor.PlayerSettings;

public enum SkillType
{
    Flash,
    MapScan
}
public class PlayerSkillManager : MonoBehaviour
{
    [SerializeField] private LayerMask m_enemyLayerMask;
    [SerializeField] private LayerMask m_wallMask;
    private QueryTriggerInteraction m_triggerInteraction = QueryTriggerInteraction.Ignore;
    [Space(10)]

    [Header(" [ OnFlashLight ] ")]
    [SerializeField] private GameObject m_flashLightObj;
    [SerializeField] private Light m_flashLight;
    [SerializeField] private float m_flashDuration = 2;
    [SerializeField] private float m_flashCoolTime = 5f;
    public float FlashDuration { get; private set; }
    public float FlashCoolTime { get; private set; }
    [Space(10)]

    [Range(0,10),SerializeField] private float m_flashLightRange = 6;   // 적 탐지 영역
    [Range(0, 50),SerializeField] private float m_flashLightAngle = 30;  // 적 감지 각도

    private List<Collider> hitRangeEnemyList = new List<Collider>();

    [Space(10)]

    [Header(" [ OnMapScan ]")]
    public float MapSacnDuration = 3;
    public float MapScanCoolTime = 8f;

    private Collider[] m_enemyColliders = new Collider[50];     //감지 오브젝트
    private List<GameObject> m_enemyList = new List<GameObject>();
    void Start()
    {

        m_flashLightObj.SetActive(false);
    }

    #region ================================================== FlashLight
    /// <summary>
    /// 들고있는 손전등의 위치로부터 3D 시야각을 통한 유령감지
    /// navMesh Stop 적용
    /// </summary>
    public void OnFlashLight()
    {
        m_flashLightObj.SetActive(true);
        StartCoroutine(OnFlashLightCoroutine());
    }

    private IEnumerator OnFlashLightCoroutine()
    {
        float m_onFlashTime = 0;

        // 일정 시간동안 체크
        while (m_onFlashTime < 1)
        {
            m_onFlashTime += Time.deltaTime / m_flashDuration;
            // FlashDuration동안 적 스캔
            FlashLightScanEnemy();
            yield return null;
        }

        m_flashLightObj.SetActive(false);
    }
    // 2. 적 감지
    private void FlashLightScanEnemy()
    {
        hitRangeEnemyList.Clear();
        // 1.범위 내 적 인식
        Vector3 _flashPos = m_flashLightObj.transform.position;
        int _enemyCount = Physics.OverlapSphereNonAlloc(_flashPos, m_flashLightRange, m_enemyColliders, m_enemyLayerMask);

        // 2. 적 위치와 시야각 각도 비교
        float halfAngle = m_flashLight.spotAngle * 0.5f;
        for (int i = 0; i < _enemyCount; i++)
        {
            Vector3 _enemyPos = m_enemyColliders[i].transform.position;
            Vector3 _enemyDir = (_enemyPos - _flashPos).normalized;
            // 손전등 앞을 기준으로 적과의 각도(180도중에)
            // 왼쪽, 오른쪽이든 정면을 기준으로 얼만큼의 각도만큼 벌어져 있는지를 _targetBeteenAngle로 반환함(그러므로 -각도가 없음)
            float _targetBeteenAngle = Vector3.Angle(m_flashLightObj.transform.forward, _enemyDir);

            // 왼쪽, 오른쪽으로의 halfAngle보다 크면 무시
            if (_targetBeteenAngle > halfAngle) continue;

            // 3.적과 손전등 사이에 장애물이 없는지 판단
            if(Physics.Raycast(_flashPos, _enemyDir,out RaycastHit hit, m_flashLightRange, m_wallMask))
            {
                if (hit.collider != null) continue; //장애물 감지시 무시
            }

            // 4. 장애물 없을 시 적에게 감지정보 전달
            IApplyFlash _enemy = m_enemyColliders[i].GetComponent<IApplyFlash>();
            _enemy.ApplyFlash(true);
        }
    }
 
    private void OnDrawGizmos()
    {
        Vector3 _flashPos = m_flashLightObj.transform.position + Vector3.up * 0.5f;
        Gizmos.DrawWireSphere(_flashPos, m_flashLightRange);
    }
    #endregion ================================================== /FlashLight

    #region ================================================== MapScan
    public void OnMapScan()
    {
        
    }
    #endregion ================================================== /MapScan
}
