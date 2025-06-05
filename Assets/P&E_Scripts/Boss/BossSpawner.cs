using UnityEngine;
using UnityEngine.SceneManagement;
using Spine.Unity;
using UnityEngine.UI;
using System.Collections;

public class BossSpawner : MonoBehaviour
{
    public static BossSpawner Instance;

    public GameObject bossPrefab;
    public string startAnimation = "Idle";
    public bool loop = true;
    public Transform spawnPoint;
    public int currentStage = 1;
    public GameObject bossDefeatedUI;
    private GameObject currentBoss;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        bossDefeatedUI.SetActive(false);
        if (bossPrefab == null)
        {
            Debug.LogError("보스 프리팹이 할당되지않음");
            return;
        }

        // Instantiate 프리팹
        Vector3 spawnPos = spawnPoint ? spawnPoint.position : transform.position;
        GameObject spawnedBoss = Instantiate(bossPrefab, spawnPos, Quaternion.identity);

        // SkeletonAnimation에서 애니메이션 실행
        SkeletonAnimation skeletonAnimation = spawnedBoss.GetComponent<SkeletonAnimation>();
        if (skeletonAnimation == null)
        {
            Debug.LogError("프리팹에 SkeletonAnimation 컴포넌트가 없습니다");
            return;
        }
        // 쉐이더 확인
        TryFixShader(skeletonAnimation);

        // 애니메이션 재생
        skeletonAnimation.AnimationState.SetAnimation(0, startAnimation, loop);
    }

    private void TryFixShader(SkeletonAnimation skeletonAnimation)
    {
        var renderer = skeletonAnimation.GetComponent<Renderer>();
        if (renderer != null && renderer.sharedMaterial != null && renderer.sharedMaterial.shader == null)
        {
            Shader spineShader = Shader.Find("Spine/Skeleton");
            if (spineShader != null)
            {
                renderer.sharedMaterial.shader = spineShader;
                Debug.Log("Spine 쉐이더 자동 설정 완료");
            }
            else
            {
                Debug.LogWarning("Spine/Skeleton 쉐이더를 찾을 수 없습니다. 수동 설정 필요.");
            }
        }
    }
    public void OnBossDefeated(GameObject boss)
    {
        StartCoroutine(BossDefeatSequence(boss));
    }

    private IEnumerator BossDefeatSequence(GameObject boss)
    {
        // 카메라 보스로 이동 및 확대
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            Vector3 bossPos = boss.transform.position;
            bossPos.y += 6f;
            bossPos.z = -10f;
            mainCam.transform.position = bossPos;
            mainCam.orthographicSize = 7f;
        }
        if (bossDefeatedUI != null)
        {
            bossDefeatedUI.SetActive(true);
        }
        PlayerPrefs.SetInt("Stage" + currentStage + "_Clear", 1);
        PlayerPrefs.SetInt("ClearStage", currentStage); // 현재 스테이지 번호 저장
        PlayerPrefs.Save();

        Time.timeScale = 0.2f;
        yield return new WaitForSecondsRealtime(5f);
        Time.timeScale = 1.0f;

        LoadClearScene();
    }

    void LoadClearScene()
    {
        SceneManager.LoadScene("Ending");
    }

    public void ClearStage()
    {
        PlayerPrefs.SetInt("Stage" + currentStage + "_Clear", 1);
        PlayerPrefs.Save();
    }
}
