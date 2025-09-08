using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BloodScreen : MonoBehaviour
{
    [SerializeField]
    private PostProcessVolume m_post;
    private float t;
    private bool m_isTwincle;
    void Update()
    {

        t += Time.deltaTime;
        if(t > 4.0f)
        {
            m_isTwincle = !m_isTwincle;
            m_post.enabled = m_isTwincle;
            t = 0;
        }
    }


}
