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
                // 타이핑 중이면 즉시 전체 텍스트 출력
                StopCoroutine(typingCoroutine);
                uiText.text = fullText;
                isTyping = false;
                textCompleted = true;
            }
            else if (textCompleted)
            {
                // 텍스트가 끝났고 한 번 더 클릭하면 씬 전환
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
