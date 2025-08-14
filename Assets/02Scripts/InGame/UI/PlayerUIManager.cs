using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public Color OffEffectColor;
    
    [Header("[ FlashUI ]")]
    [SerializeField]
    private Image m_flashIcon;
    [SerializeField]
    private Image m_flashBackgroundImg;
    [SerializeField]
    private Image m_flashCoolTimeImg;
    [Space(10)]

    [Header("[ TopViewUI ]")]
    [SerializeField]
    private Image m_topViewIcon;
    [SerializeField]
    private Image m_topViewBackgroundImg;
    [SerializeField]
    private Image m_topViewCoolTimeImg;

    private void Start()
    {
        m_flashCoolTimeImg.gameObject.SetActive(false);
        m_flashCoolTimeImg.fillAmount = 1;

        m_topViewCoolTimeImg.gameObject .SetActive(false);
        m_topViewCoolTimeImg.fillAmount = 1;
    }

    private void ChangeColor(Image icon, Image background, bool isOn)
    {
        Color _iconColor = icon.color;
        Color _backColor = background.color;

        if (isOn)
        {
            _iconColor = Color.white;
            _iconColor.a = 1;
            _backColor = Color.white;
            _backColor.a = 1;
        }
        else
        {
            _iconColor = OffEffectColor;
            _backColor = OffEffectColor;
        }
    }
    public void SkillUIOn(SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.Flash:
                ChangeColor(m_flashIcon, m_flashBackgroundImg, true);
                break;
            case SkillType.MapScan:
                ChangeColor(m_topViewIcon, m_topViewBackgroundImg, true);
                break;
        }
    }

    public void SkillUIOff(SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.Flash:
                ChangeColor(m_flashIcon, m_flashBackgroundImg, false);
                break;
            case SkillType.MapScan:
                ChangeColor(m_topViewIcon, m_topViewBackgroundImg, false);
                break;
        }
    }

    public void CoolTimeUIEffect(SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.Flash:
                break;
            case SkillType.MapScan:
                break;
        }
    }
}
