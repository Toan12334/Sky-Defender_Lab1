using UnityEngine;

public class SpikeTrapController : MonoBehaviour
{
    [Header("Cấu hình bẫy gai")]
    public GameObject spikePrefab; // Kéo Prefab bẫy gai vào đây
    public Transform spawnPoint;   // Kéo Spawn_Point 1 vào đây
    public Transform spawnPoint2;  // Kéo Spawn_Point 2 vào đây
    public float repeatRate = 2f;  // Thời gian lặp lại (2 giây)

    private bool isActivated = false; // Kiểm tra xem bẫy đã bị đạp trúng chưa

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Nếu người chơi bước vào vùng Trigger và bẫy chưa kích hoạt
        if (collision.CompareTag("Player") && !isActivated)
        {
            isActivated = true; // Đánh dấu đã kích hoạt để không bị lặp lại hàm này

            // Chạy vòng lặp: Cứ mỗi 'repeatRate' giây sẽ gọi hàm SpawnSpikes một lần
            InvokeRepeating(nameof(SpawnSpikes), 0f, repeatRate);
        }
    }

    // Đổi tên thành SpawnSpikes (số nhiều) để quản lý việc sinh ra ở cả 2 điểm
    void SpawnSpikes()
    {
        if (spikePrefab != null)
        {
            // Tạo ra gai tại vị trí điểm thứ 1 nếu có kéo vào
            if (spawnPoint != null)
            {
                Instantiate(spikePrefab, spawnPoint.position, spawnPoint.rotation);
            }

            // Tạo ra gai tại vị trí điểm thứ 2 nếu có kéo vào
            if (spawnPoint2 != null)
            {
                Instantiate(spikePrefab, spawnPoint2.position, spawnPoint2.rotation);
            }
        }
    }
}