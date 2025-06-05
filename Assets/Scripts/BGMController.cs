using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class BGMController : MonoBehaviour
{
    public AudioMixer audioMixer; // MainMixer
    public Slider bgmSlider;

    void Start()
    {
        // 저장된 값 불러오기
        float savedVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        bgmSlider.value = savedVolume;
        SetVolume(savedVolume);

        // 슬라이더 값이 변경될 때마다 호출
        bgmSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        // 볼륨 값을 로그 스케일로 변환해서 설정
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }
}
