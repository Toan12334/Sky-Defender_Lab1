using UnityEngine;
using UnityEngine.UI; // BẮT BUỘC phải có dòng này để điều khiển được Slider thanh máu

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI Reference")]
    public Slider healthSlider; // Nơi kéo thả thanh Slider vào

    void Start()
    {
        // Ban đầu lượng máu hiện tại bằng máu tối đa
        currentHealth = maxHealth;

        // Thiết lập các giá trị ban đầu cho thanh Slider
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
    }

    // Hàm này sẽ được gọi từ code của Player (khi Player chém trúng con quái này)
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // Đảm bảo lượng máu không bị âm hoặc vượt quá máu tối đa
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Cập nhật giá trị hiển thị lên thanh máu
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // Kiểm tra nếu hết máu thì chết
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Xử lý khi quái chết (ở đây tạm thời là xóa con quái khỏi map)
        Debug.Log(gameObject.name + " đã bị tiêu diệt!");
        Destroy(gameObject);
    }
}