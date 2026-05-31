using UnityEngine;
using UnityEngine.InputSystem; // Vũ khí bí mật cho Unity 6

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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Tránh lỗi nếu máy tính không nhận ra bàn phím
        if (Keyboard.current == null) return;

        // 1. Nhận phím Trái/Phải (Dùng hệ thống mới)
        float move = 0f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) move = 1f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) move = -1f;

        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        // 2. Kiểm tra chạm đất
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);

        // 3. Nhảy bằng phím Space (Dùng hệ thống mới)
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
    }
}