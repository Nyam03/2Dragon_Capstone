using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager instance;

    public Image fadeImage;
    public Text startText;
    public float fadeDuration = 1f;
    public float zoomStartSize = 2f;

    private float zoomEndSize;
    private Camera mainCam;
    private bool waitingForInput = false;
    private string targetScene;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "ClearScene") return;
        StartCoroutine(StartSequence());
    }

    IEnumerator StartSequence()
    {
        yield return new WaitForEndOfFrame();

        mainCam = Camera.main;
        if (mainCam != null && mainCam.orthographic)
        {
            zoomEndSize = mainCam.orthographicSize;
            mainCam.orthographicSize = zoomStartSize;
        }

        fadeImage.color = new Color(0, 0, 0, 1);
        startText.gameObject.SetActive(true);

        Time.timeScale = 0;
        waitingForInput = true;
    }

    private void Update()
    {
        if (waitingForInput && Input.anyKeyDown)
        {
            waitingForInput = false;
            Time.timeScale = 1;
            startText.gameObject.SetActive(false);
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float normalized = t / fadeDuration;

            float alpha = 1f - normalized;
            fadeImage.color = new Color(0f, 0f, 0f, alpha);

            if (mainCam != null && mainCam.orthographic)
            {
                mainCam.orthographicSize = Mathf.Lerp(zoomStartSize, zoomEndSize, normalized);
            }

            yield return null;
        }

        fadeImage.color = new Color(0f, 0f, 0f, 0f);
        fadeImage.gameObject.SetActive(false);
    }

    public void FadeToScene(string sceneName)
    {
        targetScene = sceneName;
        StartCoroutine(FadeOut(sceneName));
    }

    IEnumerator FadeOut(string sceneName)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = t / fadeDuration;
            fadeImage.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        // 씬 전환
        SceneManager.LoadScene(sceneName);

        // 일정 시간 후 직접 제거 (예: 2.5초)
        yield return new WaitForSeconds(2.5f);
        fadeImage.gameObject.SetActive(false);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        fadeImage.gameObject.SetActive(false);
    }
}
