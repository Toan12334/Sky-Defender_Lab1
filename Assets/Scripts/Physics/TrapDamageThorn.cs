using UnityEngine;
// Gọi namespace chứa file Health của nhân vật chính
using ThomasDev.HealthDamageSystem;

/*Author :Hungnd
 * Describe: Xử lý sát thương bẫy gai bằng cách trừ trực tiếp vào component Health của Player
 * Date:1/06/2026
*/

public class TrapDamage : MonoBehaviour
{
    [Header("Cấu hình sát thương")]
    [SerializeField] private float damageAmount = 10f; // Số máu sẽ trừ (mặc định là 10)

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu chạm đúng đối tượng có Tag là Player
        if (collision.CompareTag("Player"))
        {
            // Lấy component Health nằm trên Player vừa va chạm
            Health playerHealth = collision.GetComponent<Health>();

            if (playerHealth != null)
            {
                // Gọi hàm TakeDamage có sẵn trong file Health của Player
                playerHealth.TakeDamage(damageAmount);
                Debug.Log($"===> GAI RƠI: Đã trừ thẳng {damageAmount} HP vào component Health của Player! <===");
            }
            else
            {
                Debug.LogError("Gai đã chạm Player nhưng không tìm thấy component 'Health' trên đối tượng Player!");
            }

            // Tự hủy cái gai ngay lập tức sau khi gây sát thương để tránh đè Collider
            Destroy(gameObject);
        }
    }
}