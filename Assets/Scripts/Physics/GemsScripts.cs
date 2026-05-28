using UnityEngine;

public class GemCollect : MonoBehaviour
{
    public int scoreValue = 10; // Điểm số viên này mang lại (Xanh/Đỏ có thể chỉnh khác nhau)
    public AudioClip collectSound; // Kéo file âm thanh (.mp3/.wav) vào đây
    public GameObject effectPrefab; // Kéo Prefab Particle Effect vào nếu có

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Thêm chuyển động tự động nhẹ cho Gem (Ví dụ: Xoay tròn đều)
        transform.Rotate(Vector3.forward * 90 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem có phải Player chạm vào không (Nhớ đặt Tag "Player" cho nhân vật)
        if (collision.CompareTag("Player"))
        {
            Collect();
        }
    }

    void Collect()
    {
        

        // 2. Tạo hiệu ứng Particle (nếu có)
        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
        }

        // 3. Phát âm thanh (Phát tại vị trí hiện tại để tránh việc Destroy làm mất âm thanh)
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        // 4. Biến mất khỏi game
        Destroy(gameObject);
    }
}