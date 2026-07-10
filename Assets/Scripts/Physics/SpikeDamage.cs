using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    // Bạn có thể chỉnh lượng sát thương ngay trong bảng Inspector
    public int damageAmount = 10;

    // Biến để lưu trữ component AudioSource
    private AudioSource audioSource;

    private void Start()
    {
        // Tự động tìm và lấy component AudioSource được gắn chung trên con ếch/bẫy gai này
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogWarning("Quên chưa gắn component Audio Source vào đối tượng này rồi bạn ơi!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem vật thể chạm vào có phải là Player không
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player đã dẫm phải gai: " + collision.gameObject.name);

            // 1. Tìm script PlayerController để thực hiện nhận sát thương chuẩn
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damageAmount);
                // Lệnh này sẽ tự kích hoạt animation giật của nhân vật 
                // và tự trừ máu bên PlayerHealth luôn, cực kỳ đồng bộ!
            }

            // 2. Xử lý phát âm thanh va chạm
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
                Debug.Log("Đã phát âm thanh va chạm bẫy gai!");
            }
        }
    }
}