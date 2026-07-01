using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [Header("Cấu hình lực nảy")]
    [Tooltip("Lực đẩy nhân vật bay lên cao")]
    [SerializeField] private float bounceForce = 15f;

    [Header("Cấu hình hiệu ứng")]
    [Tooltip("Tên chính xác của Trigger hiệu ứng nhún trong Animator")]
    [SerializeField] private string animationTriggerName = "jump";

    private Animator anim;

    private void Start()
    {
        // Lấy Component Animator trên chính bàn nhún
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem đối tượng bước vào có phải là Player không thông qua Tag
        if (collision.CompareTag("Player"))
        {
            // Lấy Rigidbody2D của Player để tác dụng lực
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // Giữ nguyên vận tốc trục X, đặt lại vận tốc trục Y về 0 trước khi nảy để lực luôn đều
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, 0f);

                // Tác dụng lực hướng lên trên
                playerRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

                // Kích hoạt animation nhún nhảy nếu có Animator
                if (anim != null)
                {
                    anim.SetTrigger(animationTriggerName);
                }
            }
        }
    }
}