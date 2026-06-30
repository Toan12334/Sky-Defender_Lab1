using UnityEngine;

public class GoblinAI : MonoBehaviour
{
    private enum State { Patrol, Chase, Attack }
    [SerializeField] private State currentState = State.Patrol;

    [Header("Di Chuyển & Tuần Tra")]
    public float walkSpeed = 2f;
    public float runSpeed = 4.5f;
    public Transform pointA; // Điểm tuần tra bên trái
    public Transform pointB; // Điểm tuần tra bên phải
    private Transform targetPoint;

    [Header("Phát Hiện Player")]
    public float detectionRange = 5f;  // Khoảng cách phát hiện player
    public float attackRange = 1.2f;    // Khoảng cách để vung kiếm chém
    public LayerMask playerLayer;      // Thiết lập Layer là "Player" cho nhân vật chính
    private Transform playerTransform;

    [Header("Thành Phần Hệ Thống")]
    private Rigidbody2D rb;
    private Animator anim;
    private bool isFacingRight = true;
    private string currentAnimState; // Lưu tên animation đang chạy để tránh lặp lệnh

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Ban đầu sẽ đi hướng tới điểm B
        targetPoint = pointB;
    }

    void Update()
    {
        CheckForPlayer();

        switch (currentState)
        {
            case State.Patrol:
                PatrolBehavior();
                break;
            case State.Chase:
                ChaseBehavior();
                break;
            case State.Attack:
                AttackBehavior();
                break;
        }
    }

    // --- LOGIC CÁC TRẠNG THÁI ---

    void PatrolBehavior()
    {
        // Chuyển sang hoạt ảnh đi bộ đúng với tên trong Animator của bạn
        ChangeAnimationState("Goblin_Walking");

        // Di chuyển hướng tới điểm target
        Vector2 direction = (targetPoint.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * walkSpeed, rb.linearVelocity.y);

        // Thay đổi hướng mặt dựa vào di chuyển
        FlipTowards(targetPoint.position);

        // Nếu đến rất gần điểm đích thì đổi điểm quay lại
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.5f)
        {
            targetPoint = (targetPoint == pointA) ? pointB : pointA;
        }
    }

    void ChaseBehavior()
    {
        if (playerTransform == null) return;

        // Chuyển sang hoạt ảnh chạy đúng với tên trong Animator của bạn
        ChangeAnimationState("Goblin_Running");

        // Đuổi theo người chơi nhanh hơn
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * runSpeed, rb.linearVelocity.y);

        FlipTowards(playerTransform.position);

        // Nếu người chơi áp sát vùng tấn công thì chuyển trạng thái
        if (Vector2.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            currentState = State.Attack;
        }
    }

    void AttackBehavior()
    {
        // Dừng di chuyển khi tấn công
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        // Kích hoạt hoạt ảnh tấn công đúng với tên trong Animator của bạn
        ChangeAnimationState("Goblin_Attack");

        if (playerTransform != null)
        {
            FlipTowards(playerTransform.position);

            // Nếu người chơi chạy thoát ra khỏi tầm đánh nhưng vẫn trong tầm nhìn thì đuổi tiếp
            if (Vector2.Distance(transform.position, playerTransform.position) > attackRange)
            {
                currentState = State.Chase;
            }
        }
    }

    // --- HÀM ĐỔI HOẠT ẢNH BẰNG TÊN (TRÁNH LỖI LẶP KHUNG HÌNH) ---
    void ChangeAnimationState(string newState)
    {
        // Nếu animation này đang chạy rồi thì không gọi lại nữa (tránh bị đứng hình ở frame đầu tiên)
        if (currentAnimState == newState) return;

        // Phát animation mới với hiệu ứng chuyển đổi mượt mà (0.1 giây)
        anim.CrossFade(newState, 0.1f);

        // Cập nhật trạng thái hiện tại
        currentAnimState = newState;
    }

    // --- HÀM BỔ TRỢ ---

    // Quét tìm xem Player có nằm trong tầm ngắm không
    void CheckForPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);

        if (hit != null)
        {
            playerTransform = hit.transform;
            if (currentState == State.Patrol)
            {
                currentState = State.Chase;
            }
        }
        else
        {
            playerTransform = null;
            if (currentState != State.Patrol)
            {
                currentState = State.Patrol;
                targetPoint = (Vector2.Distance(transform.position, pointA.position) < Vector2.Distance(transform.position, pointB.position)) ? pointA : pointB;
            }
        }
    }

    void FlipTowards(Vector3 targetPosition)
    {
        if (targetPosition.x > transform.position.x && !isFacingRight)
        {
            Flip();
        }
        else if (targetPosition.x < transform.position.x && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}