using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public AudioClip bgmClip; // �ν����Ϳ��� ����
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
