using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [Header("[ FlashUI ]")]
    [SerializeField]
    private Image m_flashLightIcon;
    [SerializeField]
    private Image m_flashLightBackgroundImg;
    [SerializeField]
    private Image m_flashLightCoolTimeImg;
    [Space(10)]

    [Header("[ TopViewUI ]")]
    [SerializeField]
    private Image m_mapSacnIcon;
    [SerializeField]
    private Image m_mapScanBackgroundImg;
    [SerializeField]
    private Image m_mapScanCoolTimeImg;

    private void Start()
    {
        m_flashLightCoolTimeImg.gameObject.SetActive(false);
        m_flashLightCoolTimeImg.fillAmount = 1;

        m_mapScanCoolTimeImg.gameObject .SetActive(false);
        m_mapScanCoolTimeImg.fillAmount = 1;
    }


    public void ChargingCoolTimeUI(SkillType skillType, float fillAmount)
    {
        switch (skillType)
        {
            case SkillType.Flash:
                m_flashLightCoolTimeImg.fillAmount = fillAmount;
                if(m_mapScanCoolTimeImg.fillAmount <= 0)
                {
                    m_mapScanCoolTimeImg.fillAmount = 0;
                }
                break;
            case SkillType.MapScan:
                m_mapScanCoolTimeImg.fillAmount = fillAmount;
                if (m_mapScanCoolTimeImg.fillAmount <= 0)
                {
                    m_mapScanCoolTimeImg.fillAmount = 0;
                }
                break;
        }
    }


    public void ActivateCoolTimeUI(SkillType skillTyp, bool isActive)
    {
        switch (skillTyp)
        {
            case SkillType.Flash:
                m_flashLightCoolTimeImg.gameObject.SetActive(isActive);
                if(isActive == false)
                {
                    m_mapScanCoolTimeImg.fillAmount = 1;
                }
                break;
            case SkillType.MapScan:
                m_mapScanCoolTimeImg.gameObject.SetActive(isActive);
                if (isActive == false)
                {
                    m_mapScanCoolTimeImg.fillAmount = 1;
                }
                break;
        }
    }
}
