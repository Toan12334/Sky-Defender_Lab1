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
        // Dòng này để test xem hai vật thể đã nhận diện Trigger chưa
        Debug.Log("Vật thể chạm vào gai là: " + collision.gameObject.name);

        if (collision.CompareTag("Player"))
        {
            // 1. Xử lý trừ máu
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TakeDamage(damageAmount);
                Debug.Log("Đã trừ " + damageAmount + " máu!");
            }
            else
            {
                Debug.LogWarning("GameManager.Instance đang bị null!");
            }

            // 2. Xử lý phát âm thanh va chạm
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
                Debug.Log("Đã phát âm thanh va chạm!");
            }
        }
    }
}