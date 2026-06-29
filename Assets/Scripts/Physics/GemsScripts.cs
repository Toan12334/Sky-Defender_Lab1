/*Author :longnd
 * Describe: xuay gem, kiểm tra player chạm gem chưa, sau khi chạm gem biến mất
 * Date:23/05/2026
*/
using UnityEngine;

public class GemCollect : MonoBehaviour
{
    public AudioClip collectSound; // Kéo file âm thanh (.mp3/.wav) vào đây
    public GameObject effectPrefab; // Kéo Prefab Particle Effect vào nếu có
    public int scoreValue; // Số điểm cộng khi ăn ngọc (cấu hình trong Unity Inspector cho từng loại ngọc)

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
        // Kiểm tra xem có phải Player chạm vào không 
        if (collision.CompareTag("Player"))
        {
            Collect();
        }
    }

    void Collect()
    {
        // 1. Cộng điểm
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }

        // 2. Phát âm thanh tại vị trí Gem trước khi nó biến mất
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        // 3. Tạo hiệu ứng Particle
        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, transform.position, Quaternion.identity);
        }

        // 4. Biến mất
        Destroy(gameObject);
    }
}