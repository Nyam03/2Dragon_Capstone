using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class BGMController : MonoBehaviour
{
    public AudioMixer audioMixer; // MainMixer
    public Slider bgmSlider;

    void Start()
    {
        // ����� �� �ҷ�����
        float savedVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        bgmSlider.value = savedVolume;
        SetVolume(savedVolume);

        // �����̴� ���� ����� ������ ȣ��
        bgmSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        // ���� ���� �α� �����Ϸ� ��ȯ�ؼ� ����
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }
}
