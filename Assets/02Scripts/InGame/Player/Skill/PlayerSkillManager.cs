using UnityEngine;

public enum SkillType
{
    Flash,
    MapScan
}
public class PlayerSkillManager : MonoBehaviour
{
    [Header(" [ Flash ] ")]
    [SerializeField] private Light m_flashLight;
    [SerializeField] private float m_spotLightAngle;
    [SerializeField] private float m_lightFower;
    [SerializeField] private float m_flashRange;
    [SerializeField] private float m_flashSkillDistance;
    [SerializeField] private float m_skillRadius;
    [SerializeField] private float m_flashDuration;
    [SerializeField] private float m_flashCoolTime;

    private float m_onFlashTime;
    private float skillCoolTime;
    private bool m_isFlashCoolTime;

    private float m_skillDis;
    private float m_flashHalfAngle;

    private Vector3 leftRayDir;
    private Vector3 rightRayDir;

    void Start()
    {
        m_flashLight.gameObject.SetActive(false);
        m_flashLight.range = m_flashRange;
        m_flashLight.spotAngle = m_spotLightAngle;
    }
    private void Update()
    {
        SkillRay();
    }

    /// <summary>
    /// 거리에 따른 플래시의 펼쳐지는 각도 계산
    /// </summary>
    private void SkillRay()
    {
        leftRayDir = Quaternion.Euler(0f, -m_flashHalfAngle, 0f) * transform.forward * m_skillDis;
        rightRayDir = Quaternion.Euler(0f, m_flashHalfAngle, 0f) * transform.forward * m_skillDis;
    }
}
