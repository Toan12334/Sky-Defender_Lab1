using UnityEngine;

public class SawTrap : MonoBehaviour
{
    [Header("--- CẤU HÌNH DI CHUYỂN ---")]
    [Tooltip("Khoảng cách cưa sẽ lăn sang trái và phải tính từ vị trí gốc.")]
    public float khoangCachDiChuyen = 5f;
    [Tooltip("Tốc độ di chuyển qua lại.")]
    public float tocDoDiChuyen = 2f;

    [Header("--- CẤU HÌNH SÁT THƯƠNG ---")]
    [Tooltip("Số lượng máu người chơi bị trừ khi chạm vào.")]
    public int satThuong = 10;

    private Vector3 viTriGoc;
    private bool diSangPhai = true;

    void Start()
    {
        // Lưu lại vị trí ban đầu của cái cưa làm mốc
        viTriGoc = transform.position;
    }

    void Update()
    {
        // Tính toán giới hạn biên trái và biên phải
        float bienPhai = viTriGoc.x + khoangCachDiChuyen;
        float bienTrai = viTriGoc.x - khoangCachDiChuyen;

        // Xử lý di chuyển qua lại tự động
        if (diSangPhai)
        {
            transform.position += Vector3.right * tocDoDiChuyen * Time.deltaTime;
            if (transform.position.x >= bienPhai)
            {
                diSangPhai = false;
            }
        }
        else
        {
            transform.position += Vector3.left * tocDoDiChuyen * Time.deltaTime;
            if (transform.position.x <= bienTrai)
            {
                diSangPhai = true;
            }
        }
    }

    // Xử lý va chạm gây damage (Vì Collider của Saw đang để Is Trigger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem vật va chạm có Tag là Player hay không
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Lưỡi cưa đã chém trúng Player!");

            // 🛑 GỌI ĐẾN FILE MÁU CỦA PLAYER ĐỂ TRỪ MÁU:
            // Giả sử script quản lý máu của nhân vật bạn tên là 'PlayerHealth' 
            // và có một hàm nhận sát thương tên là 'TakeDamage(int amount)'

            /* PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(satThuong);
            }
            */
        }
    }
}