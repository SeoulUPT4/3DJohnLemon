using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Flash,
    MapScan
}
public class PlayerSkillManager : MonoBehaviour
{
    [SerializeField] private LayerMask m_enemyLayerMask;
    [SerializeField] private LayerMask m_occluderMask;         // 가림막(벽 등) 레이어
    public QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore;
    [Space(10)]

    [Header(" [ OnFlashLight ] ")]
    [SerializeField] private GameObject m_flashLightObj;
    [SerializeField] private Light m_flashLight;
    [SerializeField] private float m_flashDistance;
    [SerializeField] private float m_skillRadius;

    public float FlashLightDuration = 2;
    public float FlashLightCoolTime = 5f;
    [Space(10)]

    [Header(" [ OnMapScan ]")]
    public float MapSacnDuration = 3;
    public float MapScanCoolTime = 8f;

    private float m_skillDis;
    private float m_flashHalfAngle;

    private Collider[] m_bufferColliders = new Collider[20];     //감지 오브젝트
    private List<GameObject> m_enemyList = new List<GameObject>();
    void Start()
    {
        m_flashLightObj.SetActive(false);
    }

    public void OnFlashLight()
    {
        m_flashLightObj.SetActive(true);

        int count = Physics.OverlapSphereNonAlloc(m_flashLightObj.transform.position, m_flashDistance, m_bufferColliders, m_enemyLayerMask,triggerInteraction);







        StartCoroutine(OnFlashLightCoroutine());
    }
    private IEnumerator OnFlashLightCoroutine()
    {
        float m_onFlashTime = 0;
        m_flashHalfAngle = m_flashLight.spotAngle / 2f;
        while (m_onFlashTime < 1)
        {
            m_onFlashTime += Time.deltaTime / FlashLightDuration;

            // 거리에 따른 플래시의 펼쳐지는 각도 계산
            //Vector3 _leftRayDir = Quaternion.Euler(0f, -m_flashHalfAngle, 0f) * transform.forward * m_skillDis;
            //Vector3 _rightRayDir = Quaternion.Euler(0f, m_flashHalfAngle, 0f) * transform.forward * m_skillDis;
            
            //var colliders = Physics.SphereCastAll(transform.position, _leftRayDir,
            yield return null;
        }

        m_flashLightObj.SetActive(false);
    }

    public void OnMapScan()
    {

    }

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

    /*  //�ߺ����
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
