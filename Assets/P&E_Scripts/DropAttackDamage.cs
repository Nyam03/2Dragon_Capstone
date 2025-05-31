using UnityEngine;

public class DropAttackDamage : MonoBehaviour
{
    public int damage = 10; // ������ �ִ� ���ط�

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth health = collision.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
                Debug.Log("��Ӱ��ݸ���");
            }

            PlayerKnockback knockback = collision.GetComponent<PlayerKnockback>();
            if (knockback != null)
            {
                Vector2 knockDir = (collision.transform.position - transform.position).normalized;
                knockback.SendMessage("HandleKnockback", knockDir, SendMessageOptions.DontRequireReceiver);
            }

            Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject); // �׶��� �±׿� ������ �ٷ� ����
        }
    }
}
