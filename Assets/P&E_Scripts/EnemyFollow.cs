using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class EnemyFollow : MonoBehaviour
{
    public float moveSpeed = 4f; // �� �̵� �ӵ�
    private Transform player;
    private Animator animator;
    private Enemy enemy;
    private Boss BossEnemy;
    private bool canMove = true;
    public float followRange = 10f; // �÷��̾ ���󰡴� �ִ� �Ÿ�
    public float minFollowDistance = 1.0f;



    void Start()
    {
        // "Player" �±׸� ���� ������Ʈ ã��
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        BossEnemy = GetComponent<Boss>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float verticalDiff = player.position.y - transform.position.y;

        if (distanceToPlayer <= followRange && distanceToPlayer >= minFollowDistance && verticalDiff < 2f)
        {
            if (player != null && !enemy.isStunned && canMove)
            {
                Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
                Vector3 direction = (targetPosition - transform.position).normalized;

                transform.position += direction * moveSpeed * Time.deltaTime;
                animator.SetFloat("Speed", Mathf.Abs(direction.x * moveSpeed));
            }
            else
            {
                animator.SetFloat("Speed", 0f);
            }
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    public void StopMove()
    {
        StartCoroutine(StopMoveCoroutine());
    }

    private IEnumerator StopMoveCoroutine()
    {
        canMove = false;
        yield return new WaitForSeconds(0.5f); // ���� ����
        canMove = true;
    }
}
