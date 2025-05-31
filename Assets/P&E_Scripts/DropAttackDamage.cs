using UnityEngine;

public class DropAttackDamage : MonoBehaviour
{
    public int damage = 10; // 보스가 주는 피해량

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth health = collision.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
                Debug.Log("드롭공격맞음");
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
            Destroy(gameObject); // 그라운드 태그에 닿으면 바로 제거
        }
    }
}
