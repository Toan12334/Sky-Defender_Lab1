using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb; // Thêm Rigidbody2D để thực sự di chuyển nhân vật


    [Header("Cài đặt di chuyển")]
    public float moveSpeed = 5f;
    public float jumpForce = 15f; // Tăng lên 15 mặc định


    [Header("Cài đặt Lướt/Lộn (Dash)")]
    public float dashForce = 15f; // Tốc độ bay tới khi lộn
    public float dashDuration = 0.3f; // Thời gian một cú lộn (giây)
    public float dashCooldown = 1f; // Chờ bao lâu mới được lộn tiếp


    [Header("Cài đặt Nhảy (Better Jump)")]
    public float gravityNormal = 2f; // Trọng lực khi đứng/chạy bình thường
    public float gravityFall = 5f; // Trọng lực khi RƠI XUỐNG (rơi nhanh hơn)
    public float gravityShortJump = 8f; // Trọng lực khi nhả nút nhảy sớm (để nhảy nhẹ)


    [Header("Cài đặt phím (Có thể tùy chỉnh)")]
    public KeyCode dashKey = KeyCode.LeftShift;
    public KeyCode shieldKey = KeyCode.E;


    [Header("Cài đặt Tấn công")]
    public float attackRange = 1.2f;
    public float attackDamage = 10f;
    public float rightAttackDamageMultiplier = 1.5f; // Sát thương chuột phải nhân lên bao nhiêu lần
    public LayerMask enemyLayer; // Layer chứa kẻ địch
    public Transform attackPoint; // Vị trí tâm điểm tấn công (nếu không có sẽ tự tính)


    [Header("Cài đặt Check Mặt Đất")]
    public Transform groundCheck; // Kéo thả 1 GameObject rỗng (đặt dưới chân nhân vật) vào đây
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer; // Chọn Layer của mặt đất


    // Biến để quản lý trạng thái chạm đất thực tế
    private bool isGrounded = true;
    private SpriteRenderer sr; // Thêm biến để chứa SpriteRenderer


    // Biến cho Dash
    private float dashTimeLeft;
    private float lastDashTime = -100f;


    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>(); // Lấy component SpriteRenderer
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


            // Nới lỏng điều kiện (<= 2f) để khi bạn chạy lên dốc hoặc bị kẹt nhẹ ở góc tường
            // nó vẫn hiểu là đã chạm đất và tắt animation nhảy đi
            if (isGrounded && rb.linearVelocity.y <= 2f)
            {
                animator.SetBool("isJumping", false);
            }
        }
    }


    private void HandleMovementAndActions()
    {
        // Kiểm tra xem có đang lộn không
        if (dashTimeLeft > 0)
        {
            // Trừ dần thời gian lộn
            dashTimeLeft -= Time.deltaTime;
            // Nếu đang lộn thì KHÔNG cho phép di chuyển bằng A/D nữa để giữ nguyên tốc độ bay tới
            return;
        }


        // --- 4. Xử lý Dash (Trigger dash) ---
        // Cho phép dash nếu bấm phím và đã hết thời gian hồi chiêu (cooldown)
        if (Input.GetKeyDown(dashKey) && Time.time >= lastDashTime + dashCooldown)
        {
            animator.SetTrigger("dash");
            dashTimeLeft = dashDuration;
            lastDashTime = Time.time;

            // Tìm hướng đang quay mặt: nếu đang quay trái (flipX = true) thì hướng là -1, ngược lại là 1
            float dashDirection = sr.flipX ? -1f : 1f;

            // Đẩy nhân vật bay thẳng về phía trước (Giữ nguyên vận tốc Y để không rớt cái độp)
            if (rb != null)
            {
                rb.linearVelocity = new Vector2(dashDirection * dashForce, rb.linearVelocity.y);
            }
            return; // Lộn xong thì ngắt luôn không xử lý di chuyển ở dưới nữa
        }


        // --- 1. Xử lý Chạy/Đứng (isRunning) và Di chuyển thật ---
        float horizontalInput = Input.GetAxisRaw("Horizontal");


        if (rb != null)
        {
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        }


        // Đảo chiều nhân vật bằng cách lật hình ảnh (SpriteRenderer.flipX)
        if (sr != null)
        {
            if (horizontalInput > 0)
            {
                sr.flipX = false; // Quay mặt sang phải
            }
            else if (horizontalInput < 0)
            {
                sr.flipX = true; // Quay mặt sang trái
            }
        }


        // Cập nhật Animation
        bool isRunning = Mathf.Abs(horizontalInput) > 0;
        animator.SetBool("isRunning", isRunning);


        // --- 2. Xử lý Nhảy (isJumping) và Nhảy thật ---
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.SetBool("isJumping", true);

            if (rb != null)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }


        // --- 3. BETTER JUMP (Xử lý rơi nhanh / nhảy ngắn) ---
        if (rb != null)
        {
            if (rb.linearVelocity.y < 0)
            {
                rb.gravityScale = gravityFall;
            }
            else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.gravityScale = gravityShortJump;
            }
            else
            {
                rb.gravityScale = gravityNormal;
            }
        }


        // --- 5. Xử lý Giơ khiên (isShielding) ---
        bool isShielding = Input.GetKey(shieldKey);
        animator.SetBool("isShielding", isShielding);
    }


    private void HandleCombatInputs()
    {
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
        // Xác định vị trí đánh: nếu có attackPoint thì dùng nó, không thì tự tính khoảng cách offset từ tâm nhân vật
        Vector2 attackPos = attackPoint != null ? (Vector2)attackPoint.position : new Vector2(transform.position.x + (direction * 0.8f), transform.position.y);


        // Lấy tất cả kẻ địch trong vùng đánh
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPos, attackRange, enemyLayer);


        foreach (Collider2D enemy in hitEnemies)
        {
            bool hit = false;

            // 1. Kiểm tra script EnemyHealth tự viết của quái
            EnemyHealth customEnemyHealth = enemy.GetComponent<EnemyHealth>();
            if (customEnemyHealth != null)
            {
                customEnemyHealth.TakeDamage(finalDamage);
                hit = true;
            }

            // 2. Dự phòng kiểm tra component Health từ gói ThomasDev
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

        // Khi giơ khiên, có thể không bị mất máu hoặc chỉ bị mất một nửa máu.
        // Mở comment dòng dưới nếu bạn muốn bị mất một nửa máu khi giơ khiên:
        // if (GameManager.Instance != null && damage > 0)
        // {
        //     GameManager.Instance.TakeDamage(damage / 2);
        // }
    }


    // Gọi hàm này khi bị quái đánh trúng bình thường
    public void TakeDamage(int damage)
    {
        animator.SetTrigger("takeDamage");


        // Gọi sang GameManager để trừ máu hiển thị trên UI
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TakeDamage(damage);


            // Kiểm tra xem máu đã hết chưa để chạy animation Die
            // Lưu ý: Trong GameManager hiện tại đang tự động load scene GameOver luôn khi health <= 0
            if (GameManager.Instance.health <= 0)
            {
                Die();
            }
        }
    }


    public void Die()
    {
        animator.SetTrigger("die");
    }


    // Hàm vẽ vòng tròn đỏ trong Editor để bạn dễ dàng nhìn thấy Ground Check
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }


        // Vẽ Gizmos cho vùng tấn công để dễ tinh chỉnh trong Editor
        Gizmos.color = Color.yellow;
        Vector2 attackPosLeft = attackPoint != null ? (Vector2)attackPoint.position : new Vector2(transform.position.x - 0.8f, transform.position.y);
        Vector2 attackPosRight = attackPoint != null ? (Vector2)attackPoint.position : new Vector2(transform.position.x + 0.8f, transform.position.y);

        Gizmos.DrawWireSphere(attackPosLeft, attackRange);
        Gizmos.DrawWireSphere(attackPosRight, attackRange);
    }
}