using System.Collections;
using UnityEngine;
using Spine.Unity;

public class BossAttack : MonoBehaviour
{
    public int damage = 30;
    public float attackCooldown = 4.4f;
    public float attackDelay = 1.2f;

    private EnemyFollow enemyFollow;
    private Transform player;
    private bool isAttacking = false;
    private bool playerInRange = false;

    private SkeletonAnimation skeletonAnimation;
    private Spine.AnimationState spineAnimState;
    private Boss boss;

    private Coroutine attackRoutine;

    public AudioClip attackSound;
    private AudioSource audioSource;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyFollow = GetComponentInParent<EnemyFollow>();
        boss = GetComponentInParent<Boss>();

        skeletonAnimation = GetComponentInParent<SkeletonAnimation>();
        if (skeletonAnimation != null)
        {
            spineAnimState = skeletonAnimation.AnimationState;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //enemyFollow?.SetCanMove(false);
            playerInRange = true;
            if (attackRoutine == null)
                attackRoutine = StartCoroutine(AttackLoop());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            //if (attackRoutine != null)
            //{
            //    StopCoroutine(attackRoutine);
            //    attackRoutine = null;
            //}

            //isAttacking = false;
            //enemyFollow?.SetCanMove(true);
        }
    }

    void Attack()
    {
        if (isAttacking || player == null || spineAnimState == null) return;

        isAttacking = true;

        // 공격 애니메이션 → 아이들로 자동 복귀
        spineAnimState.SetAnimation(0, "Attack", false);
        spineAnimState.AddAnimation(0, "Idle", true, 0);

        Invoke("Damage", attackDelay);
    }

    IEnumerator AttackLoop()
    {
        while (playerInRange)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                float currentCooldown = attackCooldown;
                float currentDelay = attackDelay;

                if (boss != null && boss.IsPhase2())
                {
                    currentCooldown *= 0.5f;
                    currentDelay *= 0.5f;
                    if (skeletonAnimation != null)
                        skeletonAnimation.timeScale = 1.5f;
                }
                else
                {
                    if (skeletonAnimation != null)
                        skeletonAnimation.timeScale = 1.0f;
                }

                spineAnimState.SetAnimation(0, "Attack", false);
                spineAnimState.AddAnimation(0, "Idle", true, 0);

                yield return new WaitForSeconds(currentDelay);
                Damage();

                yield return new WaitForSeconds(currentCooldown);
                isAttacking = false;
            }
            yield return null;
        }

        // 루프가 끝나면 null로 초기화
        attackRoutine = null;
    }

    void Damage()
    {
        if (player == null || !playerInRange) return;

        if (attackSound != null)
            audioSource.PlayOneShot(attackSound);


        PlayerHealth health = player.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }

        PlayerKnockback knockback = player.GetComponent<PlayerKnockback>();
        if (knockback != null)
        {
            Vector2 knockDir = (player.position - transform.position).normalized;
            //knockback.SendMessage("HandleKnockback", knockDir, SendMessageOptions.DontRequireReceiver);
            knockback.StartKnockback(knockDir);
        }

        Invoke("ResetAttack", 0.1f);
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    public bool IsCurrentlyAttacking()
    {
        return isAttacking;
    }

    public bool IsPlayerInRange()
    {
        return player != null && attackRoutine != null;
    }
}
