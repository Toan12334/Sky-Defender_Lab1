using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    public float speed = 7f;
    public float jumpForce = 12f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private bool isGrounded;

    void Start()
    {
        // Sửa đúng cú pháp ở đây:
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        // Di chuyển
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        // Xoay mặt bằng SpriteRenderer
        if (moveInput > 0) sr.flipX = false;
        else if (moveInput < 0) sr.flipX = true;

        // Kiểm tra mặt đất
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // Điều khiển Animation
        anim.SetBool("run", Mathf.Abs(moveInput) > 0.1f);
        anim.SetBool("isGrounded", isGrounded);

        // Cập nhật trạng thái cho Animator
       
        // Nhảy
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("jump");
        }
        anim.SetBool("isGrounded", isGrounded);

        // Tự động thoát khỏi trạng thái nhảy nếu đã chạm đất
        if (isGrounded)
        {
            anim.ResetTrigger("jump"); // Ép reset trạng thái nhảy
        }
    }
}