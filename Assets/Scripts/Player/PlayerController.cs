using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    [Header("Cài đặt di chuyển")]
    public float moveSpeed = 5f;
    public float jumpForce = 15f;

    [Header("Cài đặt Lướt/Lộn (Dash)")]
    public float dashForce = 15f;
    public float dashDuration = 0.3f;
    public float dashCooldown = 1f;

    [Header("Cài đặt Nhảy (Better Jump)")]
    public float gravityNormal = 2f;
    public float gravityFall = 5f;
    public float gravityShortJump = 8f;

    [Header("Cài đặt phím (Có thể tùy chỉnh)")]
    public KeyCode dashKey = KeyCode.LeftShift;
    public KeyCode shieldKey = KeyCode.E;

    [Header("Cài đặt Tấn công")]
    public float attackRange = 1.2f;
    public float attackDamage = 10f;
    public float rightAttackDamageMultiplier = 1.5f;
    public LayerMask enemyLayer;
    public Transform attackPoint;

    [Header("Cài đặt Check Mặt Đất")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private bool isGrounded = true;
    private SpriteRenderer sr;

    private float dashTimeLeft;
    private float lastDashTime = -100f;

    // Tham chiếu tới script quản lý máu riêng của Player
    private PlayerHealth playerHealth;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        playerHealth = GetComponent<PlayerHealth>(); // Lấy script máu tự viết
    }

    void Update()
    {
        if (animator == null) return;

        CheckGrounded();
        HandleMovementAndActions();
        HandleCombatInputs();
    }

    private void CheckGrounded()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            if (isGrounded && rb.linearVelocity.y <= 2f)
            {
                animator.SetBool("isJumping", false);
            }
        }
    }

    private void HandleMovementAndActions()
    {
        if (dashTimeLeft > 0)
        {
            dashTimeLeft -= Time.deltaTime;
            return;
        }

        if (Input.GetKeyDown(dashKey) && Time.time >= lastDashTime + dashCooldown)
        {
            animator.SetTrigger("dash");
            dashTimeLeft = dashDuration;
            lastDashTime = Time.time;

            float dashDirection = sr.flipX ? -1f : 1f;

            if (rb != null)
            {
                rb.linearVelocity = new Vector2(dashDirection * dashForce, rb.linearVelocity.y);
            }
            return;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        }

        if (sr != null)
        {
            if (horizontalInput > 0) sr.flipX = false;
            else if (horizontalInput < 0) sr.flipX = true;
        }

        bool isRunning = Mathf.Abs(horizontalInput) > 0;
        animator.SetBool("isRunning", isRunning);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.SetBool("isJumping", true);

            if (rb != null)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }

        if (rb != null)
        {
            if (rb.linearVelocity.y < 0) rb.gravityScale = gravityFall;
            else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump")) rb.gravityScale = gravityShortJump;
            else rb.gravityScale = gravityNormal;
        }

        bool isShielding = Input.GetKey(shieldKey);
        animator.SetBool("isShielding", isShielding);
    }

    private void HandleCombatInputs()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("attackLeft");
            PerformAttack(-1f, attackDamage);
        }

        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("attackRight");
            PerformAttack(1f, attackDamage * rightAttackDamageMultiplier);
        }
    }

    private void PerformAttack(float direction, float finalDamage)
    {
        Vector2 attackPos = attackPoint != null ? (Vector2)attackPoint.position : new Vector2(transform.position.x + (direction * 0.8f), transform.position.y);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPos, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            bool hit = false;

            EnemyHealth customEnemyHealth = enemy.GetComponent<EnemyHealth>();
            if (customEnemyHealth != null)
            {
                customEnemyHealth.TakeDamage(finalDamage);
                hit = true;
            }

            ThomasDev.HealthDamageSystem.Health enemyHealth = enemy.GetComponent<ThomasDev.HealthDamageSystem.Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(finalDamage);
                hit = true;
            }

            if (hit)
            {
                Debug.Log($"Đánh trúng kẻ địch {enemy.name}, gây {finalDamage} sát thương!");
            }
        }
    }

    // Gọi hàm này khi bị quái đánh trúng TRONG LÚC ĐANG GIƠ KHIÊN
    public void BlockHit(int damage = 0)
    {
        animator.SetTrigger("blockHit");

        // Nếu muốn giơ khiên vẫn mất 1 nửa máu thì mở comment 3 dòng dưới:
        // if (playerHealth != null && damage > 0) {
        //     playerHealth.TakeDamage(damage / 2);
        // }
    }

    // ======================================================================
    // KHU VỰC SỬA: Điền lại code nhận sát thương, gọi trực tiếp qua PlayerHealth
    // ======================================================================
    public void TakeDamage(int damage)
    {
        if (playerHealth != null)
        {
            // Đẩy lệnh trừ máu sang file quản lý máu chuyên biệt xử lý
            playerHealth.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        Gizmos.color = Color.yellow;
        Vector2 attackPosLeft = attackPoint != null ? (Vector2)attackPoint.position : new Vector2(transform.position.x - 0.8f, transform.position.y);
        Vector2 attackPosRight = attackPoint != null ? (Vector2)attackPoint.position : new Vector2(transform.position.x + 0.8f, transform.position.y);

        Gizmos.DrawWireSphere(attackPosLeft, attackRange);
        Gizmos.DrawWireSphere(attackPosRight, attackRange);
    }
}