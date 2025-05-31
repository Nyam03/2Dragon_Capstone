using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    public float knockbackForce = 1.0f;
    public float knockbackDuration = 0.5f;

    private Rigidbody2D rb;
    private Enemy enemy;
    private Animator animator;
    private bool isKnockbacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
        animator = GetComponent<Animator>();
    }

    public void TakeKnockback(Vector2 direction)
    {
        if (!isKnockbacking)
        {
            StartCoroutine(HandleKnockback(direction));
        }
    }

    private System.Collections.IEnumerator HandleKnockback(Vector2 direction)
    {
        isKnockbacking = true;

        // 이동 정지
        enemy.isStunned = true;

        rb.velocity = Vector2.zero;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        rb.velocity = Vector2.zero;
        enemy.isStunned = false;
        isKnockbacking = false;
    }
}