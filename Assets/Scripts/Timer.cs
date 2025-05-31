using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    public GameObject GameOverPanel;
    public float time = 60f;
    private float selectTime;
    private bool isFlashing = false;
    private bool isSoundLooping = false;
    public AudioClip timerSound;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        selectTime = time;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Floor(selectTime) <= 0)
        {
            GameOverPanel.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (audioSource.isPlaying) audioSource.Stop();
        }
        else
        {
            selectTime -= Time.deltaTime;
            timerText.text = Mathf.Floor(selectTime).ToString();
            if (selectTime <= 20f)
            {
                timerText.color = Color.red;

                if (!isSoundLooping)
                {
                    audioSource.clip = timerSound;
                    audioSource.loop = true;
                    audioSource.Play();
                    isSoundLooping = true;
                }

                // 5초 이하일 때 크기 강조 반복 효과
                if (!isFlashing && selectTime <= 10f)
                {
                    isFlashing = true;
                    StartCoroutine(FlashTimerText());
                }
            }
            else
            {
                timerText.color = Color.white;
            }
        }
    }

    IEnumerator FlashTimerText()
    {
        while (selectTime > 0)
        {
            timerText.transform.localScale = Vector3.one * 1.3f;
            yield return new WaitForSeconds(0.15f);
            timerText.transform.localScale = Vector3.one;
            yield return new WaitForSeconds(0.15f);
        }
    }
}