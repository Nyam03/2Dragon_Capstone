using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class Enemy : MonoBehaviour
{
    public int currentHealth = 50;
    public int maxHealth = 50;
    public int damageAmount = 10;
    public float Stun = 0.3f;
    public bool isStunned = false;

    private Animator animator;

    public Slider enemyHealth;

    public GameObject dieParticle;

    void Start()
    {
        currentHealth = maxHealth;
        enemyHealth.maxValue = maxHealth;
        animator = GetComponent<Animator>();
        //enemyHealth = Instantiate(enemyHealth, canvas.transform).GetComponent<RectTransform>();

    }

    private void Update()
    {
        enemyHealth.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hit");
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        enemyHealth.value = currentHealth;
        Debug.Log("적이 공격을 받음! 남은 체력: " + currentHealth);

        // 넉백
        Vector2 knockDir = (transform.position - GameObject.FindWithTag("Player").transform.position).normalized;
        GetComponent<EnemyKnockback>().TakeKnockback(knockDir);

        StartCoroutine(HitStun());
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private IEnumerator HitStun()
    {
        isStunned = true;
        yield return new WaitForSeconds(Stun);
        isStunned = false;
    }

    void Die()
    {
        Debug.Log("적이 사망함!");

        GameObject particleInstance = Instantiate(dieParticle, transform.position, Quaternion.identity);
        Destroy(particleInstance, 2f); // 파티클 복제본 제거
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(5); // 체력 회복
            }
        }
        Destroy(gameObject); // 적 오브젝트 제거
    }

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Debug.Log("Enemy collided with Player!");
    //        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
    //        if (playerHealth != null)
    //        {
    //            playerHealth.TakeDamage(damageAmount);
    //        }
    //    }
    //}
}
