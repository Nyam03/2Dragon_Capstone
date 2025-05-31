using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private Vector3 originalPos;
    private Coroutine shakeRoutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            originalPos = transform.localPosition;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Shake(float duration, float magnitude)
    {
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        shakeRoutine = StartCoroutine(DoShake(duration, magnitude));
    }

    private IEnumerator DoShake(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
        shakeRoutine = null;
    }

    public void QuickShake(float magnitude)
    {
        if (shakeRoutine != null)
            StopCoroutine(shakeRoutine);

        StartCoroutine(DoQuickShake(magnitude));
    }

    private IEnumerator DoQuickShake(float magnitude)
    {
        Vector3 original = transform.localPosition;
        Vector3 offset = new Vector3(
            Random.Range(-1f, 1f) * magnitude,
            Random.Range(-1f, 1f) * magnitude,
            0
        );

        transform.localPosition = original + offset;
        yield return new WaitForSeconds(0.05f);
        transform.localPosition = original;
    }

}
