using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.AI;
using UnityEngine.UI;

public class CameraMovingByVM : MonoBehaviour
{
    public CinemachineVirtualCamera firstPersonCamera; // 1��Ī ī�޶�
    public CinemachineVirtualCamera topViewCamera; // ž�� ī�޶�
    public float transitionTime = 1.0f; // �̵� �ð�
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1); // �̵� �


    public float skillcooltime = 6.0f;
    //public TextMeshProUGUI skill;
    public TextMeshProUGUI skillC;
    //public TextMeshProUGUI time;
    //public TextMeshProUGUI skilltime;
    public Image skillOffImage;
    public Image skillCoolTimeImage;


    float coolTime =6.0f;
    float topTime = 3.0f;
    int skillCount = 3;

    private bool isMoving = false; // �̵� �� ����
    private float elapsedTime = 0.0f; // ��� �ð�
    private bool isTop = false;

    private void Start()
    {
        topViewCamera.Priority = 0;
        firstPersonCamera.Priority = 10;
        skillOffImage.gameObject.SetActive(true);
        skillCoolTimeImage.fillAmount = 0;
    }

    void Update()
    {

        //skill.GetComponent<TextMeshProUGUI>().text = "Skill : " + (int)skillCount;
        //time.GetComponent<TextMeshProUGUI>().text = "CoolTime : " + coolTime;
        //skilltime.GetComponent<TextMeshProUGUI>().text = "Skill Time : " + topTime;
        skillC.GetComponent<TextMeshProUGUI>().text = "" + (int)skillCount;

        if (isTop==true)
        {
            skillOffImage.gameObject.SetActive(true);
        }
        else
        {
            if (coolTime > 0)
            {
                coolTime -= Time.deltaTime;
                skillCoolTimeImage.fillAmount = coolTime / skillcooltime;
                skillOffImage.gameObject.SetActive(true);
            }
            else
            {
                coolTime = 0;
                skillCoolTimeImage.fillAmount = 0;
                skillOffImage.gameObject.SetActive(false);
            }
        }


        if (Input.GetKeyDown(KeyCode.C) && !isMoving && coolTime <= 0 && topViewCamera.Priority ==0&&skillCount>0)
        {
            //skillOffImage.gameObject.SetActive(true);
            skillCount--;
            topTime = 3.0f;
            isMoving = true;
            StartCoroutine(SwitchCamera());
        }

        if(isTop==true&& isMoving == false && topViewCamera.Priority == 10)
        {
            if (topTime > 0)
            {
                topTime -= Time.deltaTime;
            }
            else
            {
                topTime = 0;
                StartCoroutine(SwitchCamera());
            }
       }
    }

    IEnumerator SwitchCamera()
    {
        // 1��Ī ī�޶� -> ž�� ī�޶�� �̵�
        if (firstPersonCamera.Priority > topViewCamera.Priority)
        {
            isTop = true;
            elapsedTime = 0.0f;

            while (elapsedTime < transitionTime)
            {
                float t = curve.Evaluate(elapsedTime / transitionTime);
                firstPersonCamera.Priority = (int)(10.0f * (1.0f - t));
                topViewCamera.Priority = (int)(10.0f * t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            firstPersonCamera.Priority = 0;
            topViewCamera.Priority = 10;
        }
        // ž�� ī�޶� -> 1��Ī ī�޶�� �̵�
        else
        {
            coolTime = skillcooltime;
            elapsedTime = 0.0f;

            while (elapsedTime < transitionTime)
            {
                float t = curve.Evaluate(elapsedTime / transitionTime);
                firstPersonCamera.Priority = (int)(10.0f * t);
                topViewCamera.Priority = (int)(10.0f * (1.0f - t));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            firstPersonCamera.Priority = 10;
            topViewCamera.Priority = 0;
            isTop = false;
        }

        isMoving = false;
    }
}