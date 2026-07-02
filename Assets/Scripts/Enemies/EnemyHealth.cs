using UnityEngine;
using UnityEngine.UI;
using ThomasDev.HealthDamageSystem; // BẮT BUỘC: Thêm dòng này để gọi được script Health của Nhân vật

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Vampire Settings")]
    [Tooltip("Số máu người chơi nhận được khi tiêu diệt con quái này")]
    [SerializeField] private float healReward = 20f;

    [Header("UI Reference")]
    public Slider healthSlider;

    // ========================================================
    // THÊM MỚI: Biến để kéo thả cánh cửa từ Hierarchy vào
    // ========================================================
    [Header("Exit Door Settings")]
    [Tooltip("Kéo thả GameObject exit_door vào đây (Chỉ áp dụng cho con Rồng/Quái cuối)")]
    public GameObject exitDoor;
    // ========================================================

    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " đã bị tiêu diệt!");

        // --- ĐOẠN CODE THÊM VÀO ĐỂ HỒI MÁU CHO PLAYER ---
        // 1. Tìm đối tượng có Tag là "Player" trong màn chơi
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // 2. Lấy thành phần Health (của ThomasDev) gắn trên Player
            Health playerHealth = player.GetComponent<Health>();

            if (playerHealth != null)
            {
                // 3. Gọi hàm hồi máu và truyền số máu thưởng vào
                playerHealth.Heal(healReward);
                Debug.Log("Đã hồi " + healReward + " HP cho người chơi!");
            }
        }
        // ------------------------------------------------

        // ========================================================
        // THÊM MỚI: Kích hoạt cánh cửa xuất hiện khi quái chết
        // ========================================================
        if (exitDoor != null)
        {
            exitDoor.SetActive(true);
            Debug.Log("Cánh cửa " + exitDoor.name + " đã hiện lên!");
        }
        // ========================================================

        Destroy(gameObject);
    }
}