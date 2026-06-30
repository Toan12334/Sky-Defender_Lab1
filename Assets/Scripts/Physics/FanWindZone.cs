using UnityEngine;

public class FanWindZone : MonoBehaviour
{
    [Header("--- CẤU HÌNH LỰC ĐẨY HƯỚNG LÊN ---")]
    [Tooltip("Tốc độ lơ lửng/giữ người chơi. Nên để từ 2 đến 5. Nếu muốn đẩy bay lên cao thì để tầm 8-12.")]
    public float tocDoNangLen = 3f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Chỉ tác động khi đối tượng trong vùng gió là Player
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // Ép vận tốc trục Y của Player luôn đi lên với tốc độ mượt mà cố định
                // Điều này giúp triệt tiêu hoàn toàn lực rơi từ trên cao xuống
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, tocDoNangLen);
            }
        }
    }
}