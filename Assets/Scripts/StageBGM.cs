using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageBGM : MonoBehaviour
{
    public AudioClip[] stageBGMs; // 스테이지별 BGM
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayStageBGM();
    }

    void PlayStageBGM()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "Stage1":
                audioSource.clip = stageBGMs[0];
                break;
            case "Stage2":
                audioSource.clip = stageBGMs[1];
                break;
            case "Stage3":
                audioSource.clip = stageBGMs[2];
                break;
            default:
                Debug.LogWarning("스테이지 이름이 일치하지 않음");
                return;
        }

        audioSource.loop = true;
        audioSource.Play();
    }
}
