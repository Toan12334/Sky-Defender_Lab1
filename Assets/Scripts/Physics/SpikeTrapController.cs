using UnityEngine;

public class SpikeTrapController : MonoBehaviour
{
    [Header("Cấu hình bẫy gai")]
    public GameObject spikePrefab; // Kéo Prefab bẫy gai vào đây

    // Đổi thành mảng để chứa nhiều điểm rơi
    public Transform[] spawnPoints = new Transform[2]; // Kéo 2 Spawn_Point vào Inspector

    public float repeatRate = 2f;  // Thời gian lặp lại (2 giây)

    private bool isActivated = false; // Kiểm tra xem bẫy đã bị đạp trúng chưa

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Nếu người chơi bước vào vùng Trigger và bẫy chưa kích hoạt
        if (collision.CompareTag("Player") && !isActivated)
        {
            isActivated = true; // Đánh dấu đã kích hoạt

            // Chạy vòng lặp: Cứ mỗi 'repeatRate' giây sẽ gọi hàm SpawnSpike một lần
            InvokeRepeating(nameof(SpawnSpike), 0f, repeatRate);
        }
    }

    void SpawnSpike()
    {
        // Kiểm tra xem prefab gai và mảng các điểm rơi có hợp lệ không
        if (spikePrefab != null && spawnPoints != null && spawnPoints.Length > 0)
        {
            // Duyệt qua từng điểm một trong danh sách spawnPoints
            foreach (Transform selectedPoint in spawnPoints)
            {
                // Kiểm tra xem điểm đó đã được kéo vào Inspector chưa
                if (selectedPoint != null)
                {
                    // Tạo ra bẫy gai tại vị trí của điểm đang duyệt
                    Instantiate(spikePrefab, selectedPoint.position, selectedPoint.rotation);
                }
            }
            Debug.Log($"Đã kích hoạt thả gai đồng thời tại {spawnPoints.Length} điểm!");
        }
    }
}