using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAttack : MonoBehaviour
{
    public float detectionRange = 10f;
    public float attackCooldown = 3f;
    public float attackDelay = 3f;

    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;
    public float arrowSpeed = 10f;

    private Animator animator;
    private Transform player;
    private Transform playerTarget;
    private float lastAttackTime = -999f;

    public AudioClip bowSound;
    private AudioSource audioSource;

    private bool isAttacking = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Transform foundTarget = player.Find("PlayerTarget");
        if (foundTarget != null)
        {
            playerTarget = foundTarget;
        }
        if (arrowPrefab == null)
        {
            arrowPrefab = Resources.Load<GameObject>("Arrow");
        }
        if (arrowSpawnPoint == null)
        {
            arrowSpawnPoint = transform.Find("ArrowSpawn");
        }
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange && !isAttacking && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    void Attack()
    {
        if (isAttacking || player == null) return;

        isAttacking = true;
        lastAttackTime = Time.time;

        animator.SetTrigger("Attack");
        audioSource.PlayOneShot(bowSound);
        //Invoke(nameof(FireArrow), attackDelay);
    }

    //void FireArrow()
    //{
    //    if (arrowPrefab == null || arrowSpawnPoint == null || playerTarget == null)
    //    {
    //        isAttacking = false;
    //        return;
    //    }
    //    Vector2 direction = (playerTarget.position - arrowSpawnPoint.position).normalized;
    //    GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);

    //    // 바라보게
    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    arrow.transform.rotation = Quaternion.Euler(0f, 0f, angle);

    //    Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
    //    if (rb != null)
    //    {
    //        rb.velocity = direction * arrowSpeed;
    //    }

    //    isAttacking = false;
    //}
}
