using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public GameObject GameOverPanel;
    public Slider healthSlider; // UI 슬라이더 연결
    public AudioClip deathSound;
    public AudioClip deathSound2;
    public AudioClip takeDamageSound;
    private AudioSource AudioSource;
    private PlayerKnockback knockback;

    public Image damageOverlay;
    public float overlayFadeSpeed = 2f;
    private Coroutine flashRoutine;

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        AudioSource = GetComponent<AudioSource>(); // 오디오소스 초기화
        knockback = GetComponent<PlayerKnockback>();
    }

    public void TakeDamage(int damage)
    {
        if (knockback != null && knockback.isInvincible) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthSlider.value = currentHealth;
        AudioSource.PlayOneShot(takeDamageSound);
        StartCoroutine(FlashRed());
        CameraShake.Instance.QuickShake(0.3f);

        if (currentHealth <= 0)
        {
            StartCoroutine(DelayGameOver());
            AudioSource.PlayOneShot(deathSound2);
        }
    }

    public void Over()
    {
        GameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private IEnumerator DelayGameOver()
    {
        yield return new WaitForSeconds(0.5f);
        AudioSource.PlayOneShot(deathSound);
        Over();
    }

    private IEnumerator FlashRed()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FadeOverlay());
        yield return null;
    }

    private IEnumerator FadeOverlay()
    {
        damageOverlay.color = new Color(1, 0, 0, 0.4f); // 빨간색, 투명도 40%

        while (damageOverlay.color.a > 0f)
        {
            damageOverlay.color = new Color(1, 0, 0,
                Mathf.MoveTowards(damageOverlay.color.a, 0f, overlayFadeSpeed * Time.deltaTime));
            yield return null;
        }

        flashRoutine = null;
    }

}
