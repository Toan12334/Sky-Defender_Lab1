/*Author :Toandx
 * Describe: Day la file xu ly khi vat roi xuống ,khi rơi xuống chúng nhân vật thì nhân vật sẽ mất máu
 * Date:11/06/2026
*/

using UnityEngine;

public class FallingSpike : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isFalling = false;
    private bool hasDealtDamage = false; // Biến kiểm tra để tránh trừ máu nhiều lần trong 1 lần rơi

    [Header("Cấu hình sát thương")]
    public int damageAmount = 25; // Số lượng máu sẽ trừ khi cọc đâm trúng player

    [Header("Cấu hình âm thanh")]
    public AudioClip hitSound; // Kéo file âm thanh va chạm vào đây từ bảng Inspector

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f; // Ban đầu cọc đứng yên lơ lửng
        }
    }

    // Hàm này vẫn để hòn đá (StoneTrigger) gọi từ xa để kích hoạt rơi
    public void DropSpike()
    {
        if (isFalling) return;

        isFalling = true;
        if (rb != null)
        {
            rb.gravityScale = 3f; // Bật trọng lực để cọc lao xuống
        }
    }

    // =========================================================
    // KHU VỰC BỔ SUNG: Xử lý va chạm để trừ máu Player
    // =========================================================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. Trường hợp: Cọc rơi trúng nhân vật (Player) và chưa gây sát thương lần nào
        if (collision.gameObject.CompareTag("Player") && !hasDealtDamage)
        {
            hasDealtDamage = true; // Đánh dấu đã gây sát thương xong

            // Gọi hàm trừ máu qua PlayerController để kích hoạt mạch logic đồng bộ
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damageAmount);
            }

            // Phát âm thanh tại vị trí va chạm
            if (hitSound != null)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
            }

            // Xóa cái cọc gỗ này đi ngay lập tức sau khi trúng người
            Destroy(gameObject);
        }
        // 2. Trường hợp: Cọc rơi hụt trúng mặt đất (hoặc vật thể môi trường khác)
        else if (isFalling && !collision.gameObject.CompareTag("Player"))
        {
            if (hitSound != null && !hasDealtDamage)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
                hasDealtDamage = true; // Khóa lại để tránh kích hoạt âm thanh nhiều lần
            }

            // Chờ 0.5 giây sau khi chạm đất rồi tự xóa cọc gỗ đi cho sạch Scene
            Destroy(gameObject, 0.5f);
        }
    }
}