using UnityEngine;

public class TrapboomController : MonoBehaviour
{
    [Header("--- CẤU HÌNH SÁT THƯƠNG ---")]
    [Tooltip("Số lượng máu người chơi sẽ bị trừ khi va chạm.")]
    public int satThuong = 10;

    private Animator anim;

    void Start()
    {
        // Tự động lấy Component Animator được gắn trên đối tượng bẫy này
        anim = GetComponent<Animator>();

        if (anim == null)
        {
            Debug.LogError("Đối tượng thiếu Component Animator! Hãy kiểm tra lại.");
        }
    }

    // Hàm xử lý va chạm (Dùng cho cả Collider dạng thường hoặc Is Trigger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem vật va chạm có đúng là Player không
        if (collision.CompareTag("Player"))
        {
            ThucHienKichHoatBay(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Hỗ trợ nếu Collider của bạn không tích chọn Is Trigger
        if (collision.gameObject.CompareTag("Player"))
        {
            ThucHienKichHoatBay(collision.gameObject);
        }
    }

    private void ThucHienKichHoatBay(GameObject playerObj)
    {
        // 1. Kích hoạt chuyển Animation sang chế độ 'hit' thông qua Trigger đã tạo
        if (anim != null)
        {
            anim.SetTrigger("ActivateHit");
            Debug.Log("Đã kích hoạt Animation Hit!");
        }

        // 2. Gọi đến file quản lý máu của Player để trừ máu
        // Bạn hãy thay thế 'PlayerHealth' bằng tên Script quản lý máu thực tế của Player trong dự án của bạn
        /*
        PlayerHealth playerHealth = playerObj.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(satThuong); // Gọi hàm trừ máu
            Debug.Log("Đã gây " + satThuong + " sát thương lên Player.");
        }
        */
    }
}