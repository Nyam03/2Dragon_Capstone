using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TypewriterWithClickLegacy : MonoBehaviour
{
    public Text uiText;
    [TextArea] public string fullText;
    public float typingSpeed = 0.05f;
    public string nextSceneName;

    private bool isTyping = false;
    private bool textCompleted = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        uiText.text = "";
        typingCoroutine = StartCoroutine(TypeText());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                // Ÿ���� ���̸� ��� ��ü �ؽ�Ʈ ���
                StopCoroutine(typingCoroutine);
                uiText.text = fullText;
                isTyping = false;
                textCompleted = true;
            }
            else if (textCompleted)
            {
                // �ؽ�Ʈ�� ������ �� �� �� Ŭ���ϸ� �� ��ȯ
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    IEnumerator TypeText()
    {
        isTyping = true;
        textCompleted = false;

        StringInfo stringInfo = new StringInfo(fullText);
        int totalLength = stringInfo.LengthInTextElements;

        for (int i = 0; i < totalLength; i++)
        {
            uiText.text = stringInfo.SubstringByTextElements(0, i + 1);
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        textCompleted = true;
    }
}
