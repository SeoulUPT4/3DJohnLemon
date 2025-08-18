using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
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

    [Header(" [ DeadUI ] ")]
    [SerializeField]
    private Image m_deadBackgroundImg;
    [SerializeField]
    private Image m_deadCenterImg;

    private void Start()
    {
        m_flashLightCoolTimeImg.gameObject.SetActive(false);
        m_flashLightCoolTimeImg.fillAmount = 1;

        m_mapScanCoolTimeImg.gameObject .SetActive(false);
        m_mapScanCoolTimeImg.fillAmount = 1;

        Color _deadBackColor = m_deadBackgroundImg.color;
        _deadBackColor.a = 0f;
        m_deadBackgroundImg.color = _deadBackColor;

        Color _deadCenterColor = m_deadCenterImg.color;
        _deadCenterColor.a = 0f;
        m_deadCenterImg.color = _deadCenterColor;
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

    public void DeadUIFade(float fadeDuration)
    {
        StartCoroutine(FadeCoroutine(fadeDuration));
    }

    private IEnumerator FadeCoroutine(float fadeDuration)
    {
        yield return new WaitForSeconds(1.5f);
        float _fadeTime = 0f;
        float _fadeDuration = fadeDuration;

        while (_fadeTime <= 1)
        {
            _fadeTime += Time.deltaTime / _fadeDuration;

            Color _deadBackColor = m_deadBackgroundImg.color;
            _deadBackColor.a = _fadeTime;

            Color _deadCenterColor = m_deadCenterImg.color;
            _deadCenterColor.a = _fadeTime;

            m_deadBackgroundImg.color = _deadBackColor;
            m_deadCenterImg.color = _deadCenterColor;
            yield return null;
        }
    }
}
