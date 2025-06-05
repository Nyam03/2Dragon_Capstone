﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class Boss : MonoBehaviour
{
    public int currentHealth = 50;
    public int maxHealth = 50;
    public int damageAmount = 10;
    public float Stun = 0.3f;
    public bool isStunned = false;

    //public GameObject auraEffectPrefab;
    private bool isPhase2 = false;

    public Slider enemyHealth;
    public GameObject dieParticle;

    public float basicAttackRange = 2f;
    public float specialAttackRange = 6f;

    public GameObject dropAttackPrefab;
    public Collider2D dropArea;
    public int numberOfDrops = 3;

    public Collider2D groundCollider;
    public float jumpForce = 5f;

    private Rigidbody2D rb;
    private Transform player;

    private SkeletonAnimation skeletonAnimation;
    private Spine.AnimationState spineAnimState;

    private float specialAttackCooldown = 5f;
    private float lastSpecialAttackTime = -Mathf.Infinity;

    private BossAttack bossAttack;

    public Image jumpWarningImage; // 점프 공격 경고 이미지
    private Coroutine warningBlinkCoroutine; // 점프 경고 깜빡임 코루틴

    public Image dropWarningImage; // 드롭 공격 경고 이미지
    private Coroutine dropWarningCoroutine; // 드롭 경고 깜빡임 코루틴

    public AudioClip deathSound;
    public AudioClip dropWarningSound; // 드롭공격 경고음
    public AudioClip jumpWarningSound; // 점프공격 경고음
    public AudioClip slamImpactSound;  // 착지 사운드
    private AudioSource audioSource;

    public Image bossHPFillImage;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
        enemyHealth.maxValue = maxHealth;
        rb = GetComponent<Rigidbody2D>();

        // SkeletonAnimation 초기화
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        if (skeletonAnimation != null)
        {
            spineAnimState = skeletonAnimation.AnimationState;
            PlayAnimation("Idle", true); // 시작 상태   
        }

        dropArea = GameObject.Find("DropArea")?.GetComponent<Collider2D>();
        groundCollider = GameObject.Find("Ground")?.GetComponent<Collider2D>();
        bossAttack = GetComponentInChildren<BossAttack>();

        GameObject warningObj = GameObject.Find("JumpWarningImage");
        if (warningObj != null)
            jumpWarningImage = warningObj.GetComponent<Image>();

        GameObject dropWarningObj = GameObject.Find("DropWarningImage");
        if (dropWarningObj != null)
            dropWarningImage = dropWarningObj.GetComponent<Image>();

        if (jumpWarningImage != null)
            jumpWarningImage.enabled = false;

        if (dropWarningImage != null)
            dropWarningImage.enabled = false;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        enemyHealth.value = currentHealth;

        if (!isPhase2 && currentHealth <= maxHealth / 2)
        {
            EnterPhase2();
        }

        if (!isStunned && player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance <= basicAttackRange)
            {
                // 기본 공격은 별도 처리

            }
            else if (distance <= specialAttackRange && Time.time >= lastSpecialAttackTime + specialAttackCooldown && (bossAttack == null || !bossAttack.IsPlayerInRange()))
            {
                int choice = Random.Range(0, 2);
                if (choice == 0)
                    StartCoroutine(DropAttack());
                else
                    StartCoroutine(SlamAttack());

                lastSpecialAttackTime = Time.time;
            }
        }
    }

    private void EnterPhase2()
    {
        isPhase2 = true;
        //if (auraEffectPrefab != null)
        //{
        //    GameObject aura = Instantiate(auraEffectPrefab, transform.position, Quaternion.identity, transform);
        //    aura.transform.localPosition = Vector3.zero;
        //}

        bossHPFillImage.color = new Color(0.5f, 0, 1, 0.8f);

        Debug.Log("2페이즈");
    }

    IEnumerator DropAttack()
    {
        Debug.Log("드롭공격실행");

        // 드롭 경고 이미지 깜빡이기 (위치 변경 없이)
        if (dropWarningImage != null)
        {
            dropWarningImage.enabled = true;
            StartCoroutine(BlinkDropWarningImageForSeconds(2f)); // 2초간 깜빡이기

            if (audioSource != null && dropWarningSound != null)
                audioSource.PlayOneShot(dropWarningSound);
        }

        yield return new WaitForSeconds(0.5f);

        // 드롭 공격 실행
        if (dropArea == null) yield break;

        Bounds bounds = dropArea.bounds;
        float y = bounds.max.y;

        Vector3 playerDropPos = new Vector3(player.position.x, y, 0f);
        Instantiate(dropAttackPrefab, playerDropPos, Quaternion.Euler(0, 0, 190f));

        float minSpacing = 10.0f;
        int dropsLeft = numberOfDrops - 1;
        int attempts = 0;
        List<float> usedX = new List<float> { player.position.x };

        while (dropsLeft > 0 && attempts < 100)
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            bool isTooClose = false;

            foreach (float used in usedX)
            {
                if (Mathf.Abs(x - used) < minSpacing)
                {
                    isTooClose = true;
                    break;
                }
            }

            if (!isTooClose)
            {
                Vector3 dropPos = new Vector3(x, y, 0f);
                Instantiate(dropAttackPrefab, dropPos, Quaternion.Euler(0, 0, 190f));
                usedX.Add(x);
                dropsLeft--;
            }
            attempts++;
        }
    }


    IEnumerator SlamAttack()
    {
        Debug.Log("점프공격실행");
        PlayAnimation("Gap", false);
 
        if (rb != null)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if (jumpWarningImage != null)
        {
            jumpWarningImage.enabled = true;
            warningBlinkCoroutine = StartCoroutine(BlinkWarningImage());

            if (audioSource != null && jumpWarningSound != null)
                audioSource.PlayOneShot(jumpWarningSound);
        }

        //                  
        yield return new WaitUntil(() => rb.IsTouching(groundCollider) && rb.velocity.y <= 0f);

        if (audioSource != null && slamImpactSound != null)
            audioSource.PlayOneShot(slamImpactSound);

        if (warningBlinkCoroutine != null)
            StopCoroutine(warningBlinkCoroutine);

        if (jumpWarningImage != null)
            jumpWarningImage.enabled = false;
   
        CameraShake.Instance.Shake(0.3f, 0.5f);
 
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        if (playerCollider != null && groundCollider != null && playerCollider.IsTouching(groundCollider))
        {
            PlayerHealth health = player.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
            }

            PlayerKnockback knockback = player.GetComponent<PlayerKnockback>();
            if (knockback != null)
            {
                Vector2 knockDir = (player.position - transform.position).normalized;
                knockback.SendMessage("HandleKnockback", knockDir, SendMessageOptions.DontRequireReceiver);
            }
        }    
        PlayAnimation("Idle", true);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        enemyHealth.value = currentHealth;

        Debug.Log("보스가 피격됨! 남은 체력: " + currentHealth);

        if (bossAttack == null || !bossAttack.IsCurrentlyAttacking())
        {
            PlayAnimation("Damage taken", false);
            PlayAnimation("Idle", true, delay: 0.2f);
        }

        Vector2 knockDir = (transform.position - player.position).normalized;
        // GetComponent<EnemyKnockback>().TakeKnockback(knockDir);

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
        audioSource.PlayOneShot(deathSound);
        Debug.Log("보스가 사망함!");

        PlayAnimation("Death", false);
        if (deathSound != null)
            BossSpawner.Instance?.OnBossDefeated(gameObject);
        GameObject particleInstance = Instantiate(dieParticle, transform.position, Quaternion.identity);
        Destroy(particleInstance, 2f);
        Destroy(gameObject, 3.0f);
    }

    private void PlayAnimation(string name, bool loop, float delay = 0f)
    {
        if (spineAnimState == null) return;
        if (delay <= 0f)
            spineAnimState.SetAnimation(0, name, loop);
        else
            spineAnimState.AddAnimation(0, name, loop, delay);
    }

    public bool IsPhase2()
    {
        return isPhase2;
    }


    IEnumerator BlinkWarningImage()
    {
        while (true)
        {
            if (jumpWarningImage != null)
                jumpWarningImage.enabled = !jumpWarningImage.enabled;

            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator BlinkDropWarningImageForSeconds(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            if (dropWarningImage != null)
                dropWarningImage.enabled = !dropWarningImage.enabled;

            yield return new WaitForSeconds(0.3f); // 깜빡이는 간격 조절 가능
            timer += 0.3f;
        }

        if (dropWarningImage != null)
            dropWarningImage.enabled = false; // 깜빡임 끝나고 끔
    }

}
