using UnityEngine;
<<<<<<< Updated upstream
=======
using UnityEngine.InputSystem;
>>>>>>> Stashed changes

public class PlayerMovement : MonoBehaviour
{
    // public giúp bạn có thể chỉnh sửa tốc độ trực tiếp trên Unity Editor
    public float speed = 5f;
<<<<<<< Updated upstream
=======
    public float jumpForce = 10f;

    [Header("Kiểm Tra Mặt Đất")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float checkRadius = 0.2f;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
>>>>>>> Stashed changes

    // Update được gọi mỗi khung hình (frame)
    void Update()
    {
<<<<<<< Updated upstream
        // 1. Nhận tín hiệu từ bàn phím (A/D, Mũi tên Trái/Phải cho trục ngang; W/S, Mũi tên Lên/Xuống cho trục dọc)
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // 2. Tạo một Vector3 xác định hướng đi. (X: ngang, Y: cao/nhảy, Z: dọc)
        // Ở đây giả sử game 3D đi trên mặt phẳng nên Y = 0. Nếu game 2D, thay moveVertical vào trục Y.
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);

        // 3. Di chuyển nhân vật
        // Time.deltaTime giúp tốc độ di chuyển ổn định, không bị phụ thuộc vào độ mạnh yếu của máy tính
        transform.Translate(movement * speed * Time.deltaTime);
=======
        // 1. Kiểm tra bàn phím
        if (Keyboard.current == null) return;

        // 2. Nhận input di chuyển
        float move = 0f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) move = 1f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) move = -1f;

        // 3. Áp dụng vật lý di chuyển
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        // 4. Cập nhật tham số cho Animator
        // Sử dụng Mathf.Abs để giá trị luôn là số dương (0 hoặc 1)
        anim.SetFloat("moveSpeed", Mathf.Abs(move));

        // 5. Kiểm tra mặt đất
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        anim.SetBool("isGrounded", isGrounded);

        // 6. Xử lý nhảy
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
>>>>>>> Stashed changes
    }
}