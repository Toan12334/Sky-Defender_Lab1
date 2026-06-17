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
        // 1. Nếu cọc rơi trúng vật thể có Tag là "Player" và chưa gây sát thương lần nào
        if (collision.gameObject.CompareTag("Player") && !hasDealtDamage)
        {
            hasDealtDamage = true; // Đánh dấu đã gây sát thương xong

            // Gọi đến GameManager thông qua Instance để trừ máu nhân vật
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TakeDamage(damageAmount);
            }

            // Phát âm thanh tại vị trí va chạm trước khi đối tượng bị xóa
            if (hitSound != null)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
            }

            // Xóa cái cọc gỗ này đi ngay lập tức sau khi trúng người để không bị đè và trừ máu tiếp
            Destroy(gameObject);
        }
        // 2. Nếu cọc rơi hụt và trúng đất (hoặc bất kỳ vật thể nào khác) thì cho biến mất sau 1 khoảng thời gian cho sạch scene
        else if (isFalling)
        {
            // Phát âm thanh khi cọc rơi trúng đất (nếu bạn muốn, còn không thì có thể bỏ qua)
            if (hitSound != null && !hasDealtDamage)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
                hasDealtDamage = true; // Đánh dấu để không phát lặp lại âm thanh đất lần nữa
            }

            // Chờ 0.5 giây sau khi chạm đất rồi tự xóa cọc gỗ đi
            Destroy(gameObject, 0.5f);
        }
    }
}