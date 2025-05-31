using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClearSceneManager : MonoBehaviour
{
    public Text clearText;              // Ŭ���� �ؽ�Ʈ ������Ʈ
    public Button restartButton;        // �ٽ� ���� ��ư ������Ʈ

    void Start()
    {
        Debug.Log("ClearSceneManager.cs");
        GameObject testButton = GameObject.Find("TestButton");
        if (testButton != null)
        {
            Button btn = testButton.GetComponent<Button>();
            btn.onClick.AddListener(() => {
                Debug.Log("TestButton Ŭ����");
            });
        }
        else
        {
            Debug.Log("��ư ��ã��");
        }

        int clearedStage = PlayerPrefs.GetInt("ClearStage", 1);
        Debug.Log("ClearScene loaded. ClearStage = " + clearedStage);

        if (clearText != null)
            clearText.text = "Stage " + clearedStage + " Ŭ����!";

        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(() =>
            {
                Debug.Log("ReStartButton Ŭ���� �� StartScene �ε�");
                SceneManager.LoadScene("StartScene");
            });

            StartCoroutine(ReActivate());
        }
        else
        {
            Debug.LogWarning("restartButton�� ������� �ʾҽ��ϴ�.");
        }
    }

    IEnumerator ReActivate()
    {
        restartButton.gameObject.SetActive(false);
        yield return null;
        restartButton.gameObject.SetActive(true);
    }
}
