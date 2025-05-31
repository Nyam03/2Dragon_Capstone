using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    private bool isGrounded;
    private Rigidbody2D rb;
    private Animator animator;
    private float moveInput;
    private SpriteRenderer spriteRenderer;

    public Image flashImage;
    public float flashDuration = 0.15f;

    public GameObject slashPrefab;
    public Transform slashSpawnPoint;
    public Transform attackPoint; // 공격 범위 위치
    public Collider2D attackCollider;
    public LayerMask enemyLayers; // 공격할 대상 레이어

    public GameObject dashSlashPrefab; // Inspector에서 참격 이펙트 프리팹 연결
    public float dashDistance = 5f;
    public float dashSlashDamage = 15f;
    public float dashSlashDuration = 0.2f;

    private int comboIndex = 0;
    private float zCooldown = 0.6f;
    private float xCooldown = 5.0f;
    private float cCooldown = 5.0f;
    private float lastZTime = -999f;
    private float lastXTime = -999f;
    private float lastCTime = -999f;// 마지막 공격 시각 기록

    private bool isZAttacking = false;
    private bool isXAttacking = false;
    private bool isCAttacking = false;// 공격 중인지

    public AudioClip attackZSound;
    public AudioClip attackXSound;
    public AudioClip attackCSound;
    public AudioClip attackCSound2;
    public AudioClip jumpSound;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // 오디오소스 초기화
        // OnDrawGizmosSelected();
    }

    void Update()
    {
        // 공격 중이면 이동 불가
        //if (isAttacking) return;

        // 좌우 입력 감지
        moveInput = Input.GetAxisRaw("Horizontal");

        // 이동 방향에 따라 캐릭터 방향 변경
        if (moveInput > 0)
            spriteRenderer.flipX = false; // 오른쪽 이동 → 원래 방향
        else if (moveInput < 0)
            spriteRenderer.flipX = true;  // 왼쪽 이동 → 반전

        // 애니메이션 상태 변경
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        // 좌우 이동 처리
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // 콜라이더 방향 반전 처리
        if (attackCollider != null)
        {
            Vector3 scale = attackCollider.transform.localScale;
            scale.x = spriteRenderer.flipX ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            attackCollider.transform.localScale = scale;
        }

        // 점프 처리
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            audioSource.PlayOneShot(jumpSound);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false; // 점프하면 공중 상태로 변경
        }

        if (!isZAttacking && Time.time >= lastZTime + zCooldown && Input.GetKeyDown(KeyCode.Z))
        {
            AttackZ();
        }

        if (!isXAttacking && Time.time >= lastXTime + xCooldown && Input.GetKeyDown(KeyCode.X))
        {
            AttackX();
        }

        if (!isCAttacking && Time.time >= lastCTime + cCooldown && Input.GetKeyDown(KeyCode.C))
        {
            AttackC();
        }
    }

    // 바닥에 닿았을 때 감지
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // Ground 태그를 가진 오브젝트와 충돌했을 때
        {
            isGrounded = true;
        }

    }
    // 바닥에서 떨어질 때 감지
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void AttackZ()
    {
        isZAttacking = true;
        lastZTime = Time.time;

        // 번갈아가며 애니메이션 실행
        if (comboIndex % 2 == 0)
            animator.SetTrigger("Attack1");
        else
            animator.SetTrigger("Attack2");

        comboIndex++; // 다음에 번갈아

        rb.velocity = Vector2.zero;

        // Z 공격 사운드
        if (attackZSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackZSound);
            Debug.Log("Z 공격 사운드 재생됨");
        }

        Invoke("AttackZDelay", 0.2f); // 타격 판정
        Invoke("ResetZAttack", zCooldown);
    }

    void AttackX()
    {
        isXAttacking = true;
        lastXTime = Time.time;

        animator.SetTrigger("Attack3");
        rb.velocity = Vector2.zero;

        // X 공격 사운드
        if (attackXSound != null && audioSource != null)
            audioSource.PlayOneShot(attackXSound);

        // 참격 생성
        GameObject slash = Instantiate(slashPrefab, slashSpawnPoint.position, Quaternion.identity);

        SpriteRenderer sr = slash.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.a = 0.4f; // 40% 불투명
            sr.color = c;
        }

        // 방향 설정 (왼쪽일 경우 뒤집기)
        float direction = spriteRenderer.flipX ? -5f : 5f;
        slash.transform.localScale = new Vector3(direction, 5f, 5f);

        Rigidbody2D rbSlash = slash.GetComponent<Rigidbody2D>();
        if (rbSlash != null)
        {
            rbSlash.velocity = new Vector2(direction * 7f, 0); // 참격 속도
        }

        Invoke("ResetXAttack", xCooldown);
    }

    void AttackC()
    {
        isCAttacking = true;
        lastCTime = Time.time;

        Vector3 startPos = transform.position;
        Vector3 endPos = startPos;

        if (spriteRenderer.flipX)
            endPos.x -= dashDistance;
        else
            endPos.x += dashDistance;

        // 순간이동
        transform.position = endPos;

        // 사이 중간 위치와 길이 계산
        Vector3 centerPos = (startPos + endPos) / 2f;
        float slashWidth = Mathf.Abs(endPos.x - startPos.x);
        Vector3 scale = new Vector3(slashWidth, 10f, 1f); // 필요 시 y/z 조절

        StartCoroutine(FlashRoutine());

        // 참격 이펙트 생성
        GameObject dashSlash = Instantiate(dashSlashPrefab, centerPos, Quaternion.Euler(0, 0, -33.5f));
        dashSlash.transform.localScale = scale;

        //  C 공격 사운드
        if (attackCSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackCSound);
            audioSource.PlayOneShot(attackCSound2);
        }
            

        // 대미지 판정
        Vector2 boxSize = new Vector2(slashWidth, 1f); // 높이 조정 가능
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(centerPos, boxSize, 0f, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy e = enemy.GetComponent<Enemy>();
            if (e != null)
            {
                e.TakeDamage((int)dashSlashDamage);
            }
            else
            {
                Boss boss = enemy.GetComponent<Boss>();
                if (boss != null)
                {
                    boss.TakeDamage((int)dashSlashDamage);
                }
            }
        }

        // 일정 시간 후 이펙트 제거
        Destroy(dashSlash, dashSlashDuration);

        Invoke("ResetCAttack", cCooldown);
    }


    void AttackZDelay()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackCollider.bounds.center, attackCollider.bounds.size, 0f, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy != null && enemy.gameObject.activeInHierarchy)
            {
                Enemy e = enemy.GetComponent<Enemy>();
                if (e != null)
                {
                    e.TakeDamage(10);
                    CameraShake.Instance.QuickShake(0.1f);
                }
                else
                {
                    Boss boss = enemy.GetComponent<Boss>();
                    if (boss != null)
                    {
                        boss.TakeDamage(10);
                        CameraShake.Instance.QuickShake(0.1f);
                    }
                }
            }
        }
    }
    void ResetZAttack()
    {
        isZAttacking = false;
    }

    void ResetXAttack()
    {
        isXAttacking = false;
    }

    void ResetCAttack()
    {
        isCAttacking = false;
    }

    public bool IsZAttacking()
    {
        return isZAttacking;
    }

    private System.Collections.IEnumerator FlashRoutine()
    {
        Color c = flashImage.color;
        c.a = 1f;
        flashImage.color = c;

        float t = 0f;
        while (t < flashDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / flashDuration);
            flashImage.color = c;
            yield return null;
        }

        c.a = 0f;
        flashImage.color = c;
    }
}
