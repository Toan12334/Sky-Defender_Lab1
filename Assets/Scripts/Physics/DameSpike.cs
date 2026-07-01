using UnityEngine;
// Gọi namespace chứa file Health của nhân vật chính
using ThomasDev.HealthDamageSystem;

/*Author :Hungnd
 * Describe: Xử lý sát thương và đẩy văng (Knockback) Player khi va chạm vật lý cứng
 * Date:1/06/2026
*/

public class DameSpike : MonoBehaviour
{
    [Header("Cấu hình sát thương")]
    [SerializeField] private float damageAmount = 10f; // Số máu sẽ trừ

    [Header("Cấu hình đẩy văng (Knockback)")]
    [SerializeField] private float knockbackForce = 12f; // Lực đẩy văng nhân vật

    // Dùng OnCollisionEnter2D cho va chạm vật lý cứng
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra nếu chạm đúng đối tượng có Tag là Player
        if (collision.gameObject.CompareTag("Player"))
        {
            // 1. Trừ máu trực tiếp
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log($"===> ĐẦU CHÙY: Đã trừ {damageAmount} HP của Player!");
            }

            // 2. Xử lý đẩy văng (Knockback)
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Tính toán hướng đẩy: Từ tâm quả gai hướng thẳng sang tâm Player
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;

                // Thêm một chút lực hướng lên trên để nhân vật bị nhấc bổng lên khi văng ra
                knockbackDirection.y = Mathf.Abs(knockbackDirection.y) + 0.5f;
                knockbackDirection = knockbackDirection.normalized;

                // Đặt lại vận tốc của Player về 0 trước khi đẩy để lực tác động chuẩn xác
                playerRb.linearVelocity = Vector2.zero;

                // Tác dụng lực đẩy văng ngay lập tức
                playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                Debug.Log("===> ĐẦU CHÙY: Đã đẩy văng Player!");
            }
        }
    }
}