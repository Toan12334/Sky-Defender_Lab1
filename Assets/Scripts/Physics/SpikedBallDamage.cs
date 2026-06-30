using UnityEngine;

public class SpikedBallDamage : MonoBehaviour
{
    [Header("--- CẤU HÌNH SÁT THƯƠNG QUẢ CẦU ---")]
    [Tooltip("Số lượng máu người chơi bị trừ khi quả cầu quét qua.")]
    public int satThuong = 15;

    [Header("--- HIỆU ỨNG TRÚNG ĐÒN (NẾU CÓ) ---")]
    [Tooltip("Kéo Component Animator của quả cầu (nếu muốn nó tự nháy khi trúng). Nếu không cần thì để trống.")]
    public Animator animQuacau;

    private void Start()
    {
        // Nếu bạn không kéo thả Animator vào ô trống ở Inspector, code sẽ tự tìm trên chính nó
        if (animQuacau == null)
        {
            animQuacau = GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu va chạm trúng đối tượng có Tag là Player
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Quả cầu gai đã va trúng Player!");

            // 1. Kích hoạt hiệu ứng nháy (Hit) nếu quả cầu có thiết lập Animator Layer riêng
            if (animQuacau != null)
            {
                animQuacau.SetTrigger("ActivateHit");
            }

            // 2. Tìm Script quản lý máu trên người Player để trừ máu
            // Hãy thay 'PlayerHealth' bằng tên Script máu thực tế trong Project của bạn
            /*
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(satThuong);
            }
            */
        }
    }
}