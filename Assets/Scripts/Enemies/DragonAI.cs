using UnityEngine;
// Gọi namespace chứa hệ thống máu (Health) của bạn để script này nhận diện được class "Health" trên Player
using ThomasDev.HealthDamageSystem;

public class DragonAI : MonoBehaviour
{
    // ==========================================
    // KHAI BÁO CÁC BIẾN CẤU HÌNH (INSPECTOR)
    // ==========================================

    [Header("Patrol Settings")]
    public Transform poinA;         // Điểm tuần tra A
    public Transform poinB;         // Điểm tuần tra B
    public float patrolSpeed = 2f;  // Tốc độ di chuyển khi đi tuần tra

    [Header("Chase & Attack Settings")]
    public float chaseSpeed = 4f;       // Tốc độ di chuyển khi đuổi theo người chơi
    public float detectionRadius = 5f;  // Bán kính phát hiện người chơi (vòng tròn màu vàng)
    public float attackRadius = 2.5f;   // Bán kính để bắt đầu tấn công (vòng tròn màu đỏ)
    public LayerMask playerLayer;       // Layer của Player để bộ quét vị trí (Physics2D) lọc chính xác

    [Header("Attack Damage Settings")]
    public float damageAmount = 5f;     // Lượng sát thương gây ra cho mỗi lần khạc/đánh trúng
    public float attackRate = 1.0f;     // Giãn cách giữa các đòn đánh (1 giây đánh 1 lần)
    private float nextAttackTime = 0f;  // Mốc thời gian được phép đánh phát tiếp theo

    [Header("UI Settings")]
    public Transform healthBarCanvas;   // Thanh máu treo trên đầu Rồng (dùng để xử lý chống lật ngược UI)

    // ==========================================
    // CÁC BIẾN NỘI BỘ (PRIVATE VARIABLES)
    // ==========================================
    private Transform targetPoint;      // Điểm đến hiện tại khi tuần tra (A hoặc B)
    private Transform player;           // Lưu tọa độ của Player khi bị phát hiện
    private Animator anim;              // Component điều khiển Animation của Rồng

    // Tên các trạng thái Animation (Animation States) trong Animator của bạn
    private const string ANIM_PATROL = "Move";
    private const string ANIM_CHASE = "Climb";
    private const string ANIM_ATTACK = "SpecialAttack";

    // ==========================================
    // HÀM KHỞI TẠO (START)
    // ==========================================
    void Start()
    {
        // Lấy component Animator được gắn chung trên Object con Rồng này
        anim = GetComponent<Animator>();

        // Mới vào game, nếu có điểm A thì đặt điểm A làm mục tiêu di chuyển đầu tiên
        if (poinA != null) targetPoint = poinA;
    }

    // ==========================================
    // VÒNG LẶP CẬP NHẬT CHÍNH (UPDATE)
    // ==========================================
    void Update()
    {
        // Vẽ một vòng tròn vô hình từ tâm con Rồng, bán kính dữ theo 'detectionRadius' 
        // để quét xem có Collider nào thuộc 'playerLayer' lọt vào không
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (hit != null) // NẾU PHÁT HIỆN THẤY NGƯỜI CHƠI
        {
            player = hit.transform; // Lưu lại thông tin tọa độ của người chơi

            // Tính khoảng cách hiện tại giữa Rồng và người chơi
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRadius)
            {
                // Nếu khoảng cách nhỏ hơn hoặc bằng bán kính tấn công -> Chuyển sang Đánh
                AttackPlayer();
            }
            else
            {
                // Nếu ở xa hơn bán kính đánh nhưng vẫn nằm trong tầm nhìn -> Đuổi theo
                ChasePlayer();
            }
        }
        else // NẾU KHÔNG CÓ PLAYER TRONG TẦM NHÌN
        {
            // Rồng quay lại trạng thái đi tuần tra tự động giữa A và B
            Patrol();
        }
    }

    // ==========================================
    // CÁC HÀM XỬ LÝ TRẠNG THÁI (STATES)
    // ==========================================

    // 1. Trạng thái Tuần tra
    void Patrol()
    {
        // Nếu thiếu 1 trong 2 điểm A hoặc B thì không chạy code tuần tra nữa để tránh lỗi
        if (poinA == null || poinB == null) return;

        // Bật chuyển động di chuyển tuần tra
        anim.Play(ANIM_PATROL);

        // Di chuyển Rồng tịnh tiến từng chút một từ vị trí hiện tại đến điểm targetPoint
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, patrolSpeed * Time.deltaTime);

        // Tính toán hướng (X của Target trừ X của Rồng) để lật mặt Rồng cho đúng hướng đi
        FlipCharacter(targetPoint.position.x - transform.position.x);

        // Nếu Rồng đi gần sát tới điểm đích (khoảng cách < 0.2 đơn vị)
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            // Đổi mục tiêu: Nếu đang đi đến A thì đổi sang B, ngược lại nếu đang ở B thì đổi sang A
            targetPoint = (targetPoint == poinA) ? poinB : poinA;
        }
    }

    // 2. Trạng thái Đuổi theo người chơi
    void ChasePlayer()
    {
        // Bật animation đuổi bắt (Climb)
        anim.Play(ANIM_CHASE);

        // Di chuyển tịnh tiến thẳng tới vị trí của Player với tốc độ áp sát nhanh hơn (chaseSpeed)
        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);

        // Quay mặt Rồng nhìn về phía Player
        FlipCharacter(player.position.x - transform.position.x);
    }

    // 3. Trạng thái Tấn công người chơi
    void AttackPlayer()
    {
        // Bật animation khạc lửa / tấn công đặc biệt
        anim.Play(ANIM_ATTACK);

        // Logic bổ sung: Rồng vừa khạc vừa bám đuổi theo Player chứ không đứng yên một chỗ
        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
        FlipCharacter(player.position.x - transform.position.x);

        // Kiểm tra xem thời gian thực tế của Game đã vượt qua mốc thời gian được phép đánh tiếp theo chưa
        if (Time.time >= nextAttackTime)
        {
            // Cập nhật mốc thời gian cho lần đánh sau = Thời gian hiện tại + Giãn cách đòn đánh
            nextAttackTime = Time.time + attackRate;

            // Truy cập vào đúng linh hồn/thông số máu (Component Health) nằm trên người chơi
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                // Nếu tìm thấy component Health, gọi hàm TakeDamage truyền vào lượng dame tương ứng
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }

    // ==========================================
    // HÀM HỖ TRỢ LẬT MẶT & ĐIỀU CHỈNH UI (FLIP)
    // ==========================================
    void FlipCharacter(float directionX)
    {
        Vector3 localScale = transform.localScale;

        // Nếu hướng di chuyển qua bên phải (X dương)
        if (directionX > 0.01f)
            localScale.x = -Mathf.Abs(localScale.x); // Lật scale X âm (hoặc dương tùy thuộc hướng gốc của Sprite)
        // Nếu hướng di chuyển qua bên trái (X âm)
        else if (directionX < -0.01f)
            localScale.x = Mathf.Abs(localScale.x);

        // Áp dụng tỷ lệ scale mới cho Rồng để quay mặt
        transform.localScale = localScale;

        // XỬ LÝ CHỐNG NGƯỢC CHỮ / NGƯỢC THANH MÁU:
        // Khi Rồng lật mặt, toàn bộ UI con (Canvas) cũng bị lật theo khiến thanh máu bị đảo lộn.
        // Đoạn code này sẽ tự động đảo ngược scale X của Canvas dựa theo dấu (Sign) của Rồng để giữ UI luôn thẳng thắn.
        if (healthBarCanvas != null)
        {
            Vector3 hScale = healthBarCanvas.localScale;
            hScale.x = Mathf.Sign(transform.localScale.x) * Mathf.Abs(hScale.x);
            healthBarCanvas.localScale = hScale;
        }
    }

    // ==========================================
    // HÀM VẼ GIZMOS (CHỈ HIỂN THỊ TRONG CỬA SỔ SCENE)
    // ==========================================
    void OnDrawGizmosSelected()
    {
        // Vẽ vòng tròn màu vàng biểu thị tầm phát hiện Player
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Vẽ vòng tròn màu đỏ biểu thị tầm Rồng có thể tung đòn đánh
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}