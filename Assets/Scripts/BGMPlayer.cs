using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public AudioClip bgmClip; // 인스펙터에서 설정
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgmClip;
        audioSource.loop = true;
        //audioSource.playOnAwake = false;
        audioSource.Play();
    }
}
