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
        Debug.Log("���� ������ ����! ���� ü��: " + currentHealth);

        // �˹�
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
        Debug.Log("���� �����!");

        GameObject particleInstance = Instantiate(dieParticle, transform.position, Quaternion.identity);
        Destroy(particleInstance, 2f); // ��ƼŬ ������ ����
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(5); // ü�� ȸ��
            }
        }
        Destroy(gameObject); // �� ������Ʈ ����
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
