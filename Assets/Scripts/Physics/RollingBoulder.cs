using UnityEngine;

public class RollingBoulder : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float rollForce = 5f; // Lực đẩy đá ban đầu
    private bool hasTriggered = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void TriggerBoulder()
    {
        if (!hasTriggered)
        {
            hasTriggered = true;

            // Chuyển đá sang Dynamic để tính toán vật lý rơi
            rb.bodyType = RigidbodyType2D.Dynamic;

            // Đẩy nhẹ quả đá sang trái
            rb.AddForce(Vector2.left * rollForce, ForceMode2D.Impulse);

            // Kích hoạt Trigger để chuyển trạng thái từ Idle sang daAnimation
            Animator anim = GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("Roll"); // Chữ "Roll" phải viết hoa viết thường giống hệt tên bên tab Parameters
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Nếu quả đá lăn đè trúng nhân vật
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            if (player != null)
            {
                // Thay vì gọi trực tiếp Die() đang bị private, ta trừ thẳng lượng máu lớn (bằng maxHealth)
                // Hàm TakeDamage bên PlayerHealth sẽ tự động kích hoạt animation takeDamage, đưa máu về 0 và tự gọi Die() luôn!
                player.TakeDamage(player.maxHealth);

                Debug.Log("Hòn đá đã đè bẹp Player!");
            }
        }
    }
}