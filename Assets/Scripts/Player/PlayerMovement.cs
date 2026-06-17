using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Di Chuyển")]
    public float speed = 5f;
    public float jumpForce = 10f;

    [Header("Kiểm Tra Mặt Đất")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float checkRadius = 0.2f;

    private Rigidbody2D rb;
    private bool isGrounded;

    //  THÊM VÀO: Khai báo biến Animator
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //  THÊM VÀO: Lấy thành phần Animator từ nhân vật
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // 1. Nhận phím Trái/Phải
        float move = 0f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) move = 1f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) move = -1f;

        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        //  THÊM VÀO: Truyền trạng thái chạy sang Animator
        // Nếu move khác 0 (tức là đang bấm nút đi) thì run = true, ngược lại run = false
        if (anim != null)
        {
            anim.SetBool("run", move != 0f);

            // Đồng thời truyền luôn trạng thái Nhảy dựa vào việc có chạm đất hay không
            anim.SetBool("jump", !isGrounded);
        }

        // 2. Kiểm tra chạm đất
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // 3. Nhảy bằng phím Space
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        }
    }
}