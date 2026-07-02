using UnityEngine;
// Gọi namespace hệ thống máu của bạn để sử dụng được class Health
using ThomasDev.HealthDamageSystem;

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

    [Header("Cấu Hình Sát Thương")]
    public float damageAmount = 5f;    // Số máu Player bị trừ (đồng bộ kiểu float)
    public float attackRate = 1f;      // Tốc độ đánh (1 giây chém 1 lần)
    private float nextAttackTime = 0f; // Bộ đếm thời gian hồi chiêu

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
        ChangeAnimationState("Goblin_Walking");

        Vector2 direction = (targetPoint.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * walkSpeed, rb.linearVelocity.y);

        FlipTowards(targetPoint.position);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.5f)
        {
            targetPoint = (targetPoint == pointA) ? pointB : pointA;
        }
    }

    void ChaseBehavior()
    {
        if (playerTransform == null) return;

        ChangeAnimationState("Goblin_Running");

        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * runSpeed, rb.linearVelocity.y);

        FlipTowards(playerTransform.position);

        if (Vector2.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            currentState = State.Attack;
        }
    }

    void AttackBehavior()
    {
        // Dừng di chuyển khi tấn công
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        // Kích hoạt hoạt ảnh tấn công
        ChangeAnimationState("Goblin_Attack");

        if (playerTransform != null)
        {
            FlipTowards(playerTransform.position);

            // XỬ LÝ GÂY SÁT THƯƠNG: Kiểm tra nếu đã hết thời gian hồi chiêu chém
            if (Time.time >= nextAttackTime)
            {
                // Sử dụng OverlapCircle ngay tại tầm đánh để quét tìm chính xác Player trong phạm vi chém
                Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);

                if (hitPlayer != null)
                {
                    // Gọi hàm xử lý trừ máu
                    DealDamageToPlayer(hitPlayer.gameObject);
                }

                // Đặt thời gian hồi chiêu cho phát chém tiếp theo
                nextAttackTime = Time.time + attackRate;
            }

            // Nếu người chơi chạy thoát ra khỏi tầm đánh nhưng vẫn trong tầm nhìn thì đuổi tiếp
            if (Vector2.Distance(transform.position, playerTransform.position) > attackRange)
            {
                currentState = State.Chase;
            }
        }
    }

    // --- HÀM TÌM SCRIPT HEALTH VÀ TRỪ MÁU ---
    void DealDamageToPlayer(GameObject playerObj)
    {
        // Tìm component Health trực tiếp từ đối tượng quét trúng
        Health playerHealth = playerObj.GetComponent<Health>();

        if (playerHealth != null)
        {
            // Gọi hàm TakeDamage có sẵn trong script Health của bạn
            playerHealth.TakeDamage(damageAmount);
            Debug.Log($"<color=red>[GOBLIN ATTACK]</color> Chém trúng! Player bị trừ {damageAmount} máu. Máu hiện tại: {playerHealth.CurrentHealth}");
        }
        else
        {
            Debug.LogWarning("Tìm thấy đối tượng ở Layer Player nhưng không có component 'Health' trên đối tượng đó!");
        }
    }

    // --- HÀM ĐỔI HOẠT ẢNH BẰNG TÊN ---
    void ChangeAnimationState(string newState)
    {
        if (currentAnimState == newState) return;
        anim.CrossFade(newState, 0.1f);
        currentAnimState = newState;
    }

    // --- HÀM BỔ TRỢ ---

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