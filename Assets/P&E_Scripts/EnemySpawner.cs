using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnSetting
    {
        public Transform spawnPoint;
        public GameObject enemyPrefab;
        public int spawnCount; // 각 적 종류의 개수
    }

    public SpawnSetting[] spawnSettings;
    public float spawnInterval = 3f;

    private float timer = 0f;
    private int settingIndex = 0;
    private int currentSpawnedForType = 0;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    public int currentStage = 1; // 스테이지 번호

    void Update()
    {
        if (settingIndex < spawnSettings.Length)
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                SpawnEnemy();
                timer = 0f;
            }
        }

        CheckEnemiesAlive();
    }

    void SpawnEnemy()
    {
        if (settingIndex >= spawnSettings.Length) return;

        var setting = spawnSettings[settingIndex];

        // Boss 컴포넌트가 있으면 SpawnBoss로 분기
        if (setting.enemyPrefab.GetComponent<Boss>() != null)
        {
            SpawnBoss(setting.enemyPrefab, setting.spawnPoint);
        }
        else
        {
            GameObject enemy = Instantiate(setting.enemyPrefab, setting.spawnPoint.position, Quaternion.identity);
            spawnedEnemies.Add(enemy);
        }

        currentSpawnedForType++;
        if (currentSpawnedForType >= setting.spawnCount)
        {
            settingIndex++;
            currentSpawnedForType = 0;
        }
    }

    public void SpawnBoss(GameObject bossPrefab, Transform spawnPoint)
    {
        GameObject bossInstance = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
        spawnedEnemies.Add(bossInstance);

        Boss bossScript = bossInstance.GetComponent<Boss>();
        if (bossScript != null)
        {
            // DropArea 하나에서 범위 + Y위치 참조
            GameObject dropAreaObj = GameObject.Find("DropArea");
            GameObject groundObj = GameObject.Find("Ground");

            if (dropAreaObj != null)
            {
                Collider2D dropCollider = dropAreaObj.GetComponent<Collider2D>();
                if (dropCollider != null)
                {
                    bossScript.dropArea = dropCollider;
                }
                else
                {
                    Debug.LogWarning("DropArea에 Collider2D가 없습니다.");
                }
            }
            else
            {
                Debug.LogWarning("DropArea 오브젝트를 찾을 수 없습니다.");
            }

            if (groundObj != null)
            {
                Collider2D groundCol = groundObj.GetComponent<Collider2D>();
                if (groundCol != null)
                {
                    bossScript.groundCollider = groundCol;
                }
                else
                {
                    Debug.LogWarning("Ground에 Collider2D가 없습니다.");
                }
            }
            else
            {
                Debug.LogWarning("Ground 오브젝트를 찾을 수 없습니다.");
            }
        }
    }


    void CheckEnemiesAlive()
    {
        spawnedEnemies.RemoveAll(enemy => enemy == null);

        bool allSpawned = settingIndex >= spawnSettings.Length;
        if (allSpawned && spawnedEnemies.Count == 0)
        {
            PlayerPrefs.SetInt("Stage" + currentStage + "_Clear", 1);
            PlayerPrefs.SetInt("ClearStage", currentStage); // 현재 스테이지 번호 저장
            PlayerPrefs.Save();

            Invoke("LoadClearScene", 1.0f);
        }
    }

    void LoadClearScene()
    {
        SceneManager.LoadScene("ClearScene");
    }

    public void ClearStage()
    {
        PlayerPrefs.SetInt("Stage" + currentStage + "_Clear", 1);
        PlayerPrefs.Save();
    }
}
