/*Author : Toandx
 * Describe: Day la file xu ly khi nhan vat cham nuoc thi se chet ngay lap tuc
 * Date: 11/06/2026
*/
using UnityEngine;
using ThomasDev.HealthDamageSystem; // BẮT BUỘC: Gọi namespace này để sử dụng được script Health của người chơi

public class WaterScripts : MonoBehaviour
{
    // === DÀNH CHO GAME 2D ===
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem đối tượng va chạm có phải là Người chơi (Player) không
        if (collision.CompareTag("Player"))
        {
            // Lấy script Health từ người chơi
            Health playerHealth = collision.GetComponent<Health>();

            if (playerHealth != null)
            {
                playerHealth.Kill(); // Hàm Kill() có sẵn trong script Health của bạn sẽ đưa máu về 0 và kích hoạt OnDeath
                Debug.Log("Người chơi đã lọt hố nước và tử trận!");
            }
        }
    }

    // === DÀNH CHO GAME 3D (Nếu game của bạn là 3D thì dùng hàm này, nếu là 2D thì xóa đi cũng được) ===
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.Kill();
                Debug.Log("Người chơi đã lọt hố nước và tử trận!");
            }
        }
    }
}