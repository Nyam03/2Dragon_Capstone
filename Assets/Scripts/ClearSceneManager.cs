using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClearSceneManager : MonoBehaviour
{
    public Text clearText;              // 클리어 텍스트 오브젝트
    public Button restartButton;        // 다시 시작 버튼 오브젝트

    void Start()
    {
        Debug.Log("ClearSceneManager.cs");
        GameObject testButton = GameObject.Find("TestButton");
        if (testButton != null)
        {
            Button btn = testButton.GetComponent<Button>();
            btn.onClick.AddListener(() => {
                Debug.Log("TestButton 클릭됨");
            });
        }
        else
        {
            Debug.Log("버튼 못찾음");
        }

        int clearedStage = PlayerPrefs.GetInt("ClearStage", 1);
        Debug.Log("ClearScene loaded. ClearStage = " + clearedStage);

        if (clearText != null)
            clearText.text = "Stage " + clearedStage + " 클리어!";

        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(() =>
            {
                Debug.Log("ReStartButton 클릭됨 → StartScene 로드");
                SceneManager.LoadScene("StartScene");
            });

            StartCoroutine(ReActivate());
        }
        else
        {
            Debug.LogWarning("restartButton이 연결되지 않았습니다.");
        }
    }

    IEnumerator ReActivate()
    {
        restartButton.gameObject.SetActive(false);
        yield return null;
        restartButton.gameObject.SetActive(true);
    }
}
