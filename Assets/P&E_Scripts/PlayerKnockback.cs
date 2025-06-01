using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.5f;
    public float invincibleDuration = 1.0f;
    public float blinkInterval = 0.1f; // ������ ����

    private Rigidbody2D rb;
    public bool isInvincible = false;
    //private PlayerController playerMovement;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //playerMovement = GetComponent<PlayerController>(); // ���� ��ũ��Ʈ
        spriteRenderer = GetComponent<SpriteRenderer>(); // �������� ���� ��������Ʈ
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

        // ���� ��Ȱ��ȭ
        //if (playerMovement != null)
            //playerMovement.enabled = false;

        // �˹�
        rb.velocity = Vector2.zero;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        // �����̱� ����
        StartCoroutine(BlinkEffect());

        // �˹� ���� �ð� ��ٸ�
        yield return new WaitForSeconds(knockbackDuration);

        // ���� �ٽ� Ȱ��ȭ
        //if (playerMovement != null)
            //playerMovement.enabled = true;

        // ���� �ð� ������ ��ٸ�
        yield return new WaitForSeconds(invincibleDuration - knockbackDuration);
        isInvincible = false;

        // ������ ����: ���� ������ ����
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
                // ���� <-> ������ �ݺ�
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
