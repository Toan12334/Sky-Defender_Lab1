using UnityEngine;
using ThomasDev.HealthDamageSystem; // Sử dụng đúng namespace máu của bạn

/*Author :Toandx & Hungnd
 * Describe: Xử lý bẫy đá gai: Va chạm đổi sang hiệu ứng boom1 + trừ máu, rời ra quay về trạng thái cũ boom
 * Date:11/06/2026
*/

public class SpikeHeadTrap : MonoBehaviour
{
    [Header("Cấu hình sát thương")]
    [SerializeField] private float damageAmount = 15f; // Lượng máu trừ khi dẫm vào bẫy

    private Animator anim;

    private void Start()
    {
        // Lấy component Animator nằm trên chính cái bẫy này
        anim = GetComponent<Animator>();
    }

    // Khi Player bước vào vùng bẫy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 1. Bật animation boom1 lên thông qua biến Bool
            if (anim != null)
            {
                anim.SetBool("isActivated", true);
            }

            // 2. Trừ máu trực tiếp từ file Health của nhân vật
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                Debug.Log($"===> BẪY ĐÁ GAI: Player dẫm phải bẫy, trừ {damageAmount} HP!");
            }
        }
    }

    // Khi Player bước ra khỏi vùng bẫy
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Tắt hiệu ứng kích hoạt, tự động quay trở về trạng thái cũ (boom)
            if (anim != null)
            {
                anim.SetBool("isActivated", false);
            }
            Debug.Log("===> BẪY ĐÁ GAI: Player đã rời đi, bẫy thu hồi!");
        }
    }
}