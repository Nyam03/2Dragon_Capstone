using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.5f;
    public float invincibleDuration = 1.0f;
    public float blinkInterval = 0.1f; // 깜빡임 간격

    private Rigidbody2D rb;
    public bool isInvincible = false;
    //private PlayerController playerMovement;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //playerMovement = GetComponent<PlayerController>(); // 조작 스크립트
        spriteRenderer = GetComponent<SpriteRenderer>(); // 깜빡임을 위한 스프라이트
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (!isInvincible && collision.gameObject.CompareTag("Enemy"))
    //    {
    //        Vector2 knockbackDir = (transform.position - collision.transform.position).normalized;
    //        StartCoroutine(HandleKnockback(knockbackDir));
    //    }
    //}

    private System.Collections.IEnumerator HandleKnockback(Vector2 direction)
    {
        isInvincible = true;

        // 조작 비활성화
        //if (playerMovement != null)
            //playerMovement.enabled = false;

        // 넉백
        rb.velocity = Vector2.zero;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        // 깜빡이기 시작
        StartCoroutine(BlinkEffect());

        // 넉백 지속 시간 기다림
        yield return new WaitForSeconds(knockbackDuration);

        // 조작 다시 활성화
        //if (playerMovement != null)
            //playerMovement.enabled = true;

        // 무적 시간 끝까지 기다림
        yield return new WaitForSeconds(invincibleDuration - knockbackDuration);
        isInvincible = false;

        // 깜빡임 종료: 원래 색으로 복원
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 1f;
            spriteRenderer.color = c;
        }
    }

    private System.Collections.IEnumerator BlinkEffect()
    {
        while (isInvincible)
        {
            if (spriteRenderer != null)
            {
                // 투명 <-> 불투명 반복
                Color c = spriteRenderer.color;
                c.a = (c.a == 1f) ? 0.3f : 1f;
                spriteRenderer.color = c;
            }
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    public void StartKnockback(Vector2 direction)
    {
        if (!isInvincible)
            StartCoroutine(HandleKnockback(direction));
    }
}
