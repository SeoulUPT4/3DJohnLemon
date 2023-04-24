using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class CameraMovingByVM : MonoBehaviour
{
    public CinemachineVirtualCamera firstPersonCamera; // 1인칭 카메라
    public CinemachineVirtualCamera topViewCamera; // 탑뷰 카메라
    public float transitionTime = 1.0f; // 이동 시간
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1); // 이동 곡선

    public TextMeshProUGUI skill;
    public TextMeshProUGUI time;
    public TextMeshProUGUI skilltime;
    float coolTime = 6.0f;
    public float topTime = 3.0f;
    int skillCount = 3;

    private bool isMoving = false; // 이동 중 여부
    private float elapsedTime = 0.0f; // 경과 시간
    private bool isTop = false;

    private void Start()
    {
        topViewCamera.Priority = 0;
        firstPersonCamera.Priority = 10;
    }

    void Update()
    {

        skill.GetComponent<TextMeshProUGUI>().text = "Skill : " + (int)skillCount;
        time.GetComponent<TextMeshProUGUI>().text = "CoolTime : " + coolTime;
        skilltime.GetComponent<TextMeshProUGUI>().text = "Skill Time : " + topTime;
        if (coolTime>0)
        {
            coolTime -= Time.deltaTime;
        }
        else
        {
            coolTime = 0;
        }

        if (Input.GetKeyDown(KeyCode.C) && !isMoving && coolTime <= 0 && topViewCamera.Priority ==0&&skillCount>0)
        {
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
        // 1인칭 카메라 -> 탑뷰 카메라로 이동
        if (firstPersonCamera.Priority > topViewCamera.Priority)
        {
            coolTime = 10f;
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
            isTop = true;
        }
        // 탑뷰 카메라 -> 1인칭 카메라로 이동
        else
        {
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