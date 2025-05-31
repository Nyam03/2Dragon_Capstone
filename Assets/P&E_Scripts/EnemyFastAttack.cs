using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFastAttack : MonoBehaviour
{
    public int damage = 10;
    public float attackCooldown = 1.0f;
    public float attackDelay = 0.3f;

    public Animator animator;
    private EnemyFollow enemyFollow;

    private Transform player;
    private bool isAttacking = false;

    public AudioClip ninjaSound;
    private AudioSource audioSource;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyFollow = GetComponentInParent<EnemyFollow>();
        audioSource = GetComponent<AudioSource>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemyFollow.SetCanMove(false);
            InvokeRepeating("Attack", attackDelay, attackCooldown);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CancelInvoke("Attack");
            isAttacking = false;
            enemyFollow.SetCanMove(true);
        }
    }

    void Attack()
    {
        if (isAttacking || player == null) return;

        isAttacking = true;
        animator.SetTrigger("Attack");
        audioSource.PlayOneShot(ninjaSound);
        Invoke("Damage", attackDelay);
    }

    void Damage()
    {
        if (player == null) return;

        // 데미지
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }

        // 넉백
        PlayerKnockback knockback = player.GetComponent<PlayerKnockback>();
        if (knockback != null)
        {
            Vector2 knockDir = (player.position - transform.position).normalized;
            //knockback.SendMessage("HandleKnockback", knockDir, SendMessageOptions.DontRequireReceiver);
            knockback.StartKnockback(knockDir);
        }

        // 다음 공격 가능
        Invoke("ResetAttack", 0.1f);
    }

    void ResetAttack()
    {
        isAttacking = false;
    }
}
