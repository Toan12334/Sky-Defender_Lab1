using UnityEngine;
using UnityEngine.InputSystem; // Sử dụng hệ thống Input System mới của Unity

public class PlayerMovement : MonoBehaviour
{
    [Header("Di Chuyển")]
    public float speed = 5f;       // Tốc độ di chuyển ngang của nhân vật
    public float jumpForce = 10f;  // Lực nhảy của nhân vật

    [Header("Kiểm Tra Mặt Đất")]
    public Transform groundCheck;  // Vị trí đặt vật thể kiểm tra va chạm đất (thường ở chân nhân vật)
    public LayerMask groundLayer;  // Lớp (Layer) được coi là mặt đất
    public float checkRadius = 0.2f; // Bán kính của vòng tròn kiểm tra mặt đất

    private Rigidbody2D rb;        // Thành phần xử lý vật lý 2D
    private Animator anim;         // Thành phần xử lý hoạt ảnh (animation)
    private SpriteRenderer sr;     // Thành phần xử lý hiển thị hình ảnh (Sprite)
    private bool isGrounded;       // Biến trạng thái xem nhân vật có đang đứng trên đất không

    // Hàm Start được gọi một lần duy nhất khi game bắt đầu
    void Start()
    {
        // Lấy và lưu trữ các thành phần (Components) được gắn trên cùng GameObject này
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Hàm Update được gọi liên tục sau mỗi khung hình (Frame)
    void Update()
    {
        // Nếu không tìm thấy bàn phím nào đang kết nối thì dừng xử lý để tránh lỗi
        if (Keyboard.current == null) return;

        // --- XỬ LÝ DI CHUYỂN NGANG ---
        float moveInput = 0f;
        // Kiểm tra nếu bấm phím D hoặc mũi tên phải thì hướng di chuyển là sang phải (1)
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput = 1f;
        // Kiểm tra nếu bấm phím A hoặc mũi tên trái thì hướng di chuyển là sang trái (-1)
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveInput = -1f;

        // Áp dụng vận tốc di chuyển vào Rigidbody2D (giữ nguyên vận tốc trục Y hiện tại)
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        // --- XOAY HƯỚNG SPRITE (FLIP) ---
        // Sử dụng thuộc tính flipX giúp nhân vật quay mặt sang trái/phải mà không ảnh hưởng tới Local Scale của Object cha
        if (moveInput > 0)
            sr.flipX = false; // Không lật hình (mặt quay sang phải)
        else if (moveInput < 0)
            sr.flipX = true;  // Lật hình theo trục X (mặt quay sang trái)

        // --- CẬP NHẬT HOẠT ẢNH (ANIMATION) ---
        if (anim != null)
        {
            // Nếu moveInput khác 0 (đang bấm nút đi) thì bật animation "run"
            anim.SetBool("run", moveInput != 0f);
            // Nếu KHÔNG đứng trên mặt đất (!isGrounded) thì bật animation "jump"
            anim.SetBool("jump", !isGrounded);
        }

        // --- KIỂM TRA MẶT ĐẤT (GROUND CHECK) ---
        // Tạo một vòng tròn ảo tại vị trí groundCheck, nếu nó chạm vào bất kỳ Collider nào thuộc lớp groundLayer thì trả về true
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // --- XỬ LÝ NHẢY ---
        // Nếu vừa nhấn phím Space trong khung hình này VÀ nhân vật đang đứng trên đất
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            // Thay đổi vận tốc trục Y để đẩy nhân vật lên cao (giữ nguyên vận tốc trục X)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    // Hàm vẽ các công cụ hỗ trợ trong cửa sổ Scene của Unity (Không xuất hiện khi chạy Game thực tế)
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            // Đặt màu đỏ cho nét vẽ
            Gizmos.color = Color.red;
            // Vẽ một vòng tròn dạng khung dây (Wire) để dễ dàng căn chỉnh bán kính checkRadius trực quan hơn
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}