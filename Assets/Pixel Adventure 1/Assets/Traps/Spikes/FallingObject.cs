using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // Đảm bảo đối tượng luôn có Rigidbody2D
[RequireComponent(typeof(BoxCollider2D))] // Đảm bảo đối tượng luôn có BoxCollider2D
public class FallingObject : MonoBehaviour
{
    // Đưa biến này ra ngoài để bạn có thể tự nhập số ở bảng Inspector
    [Header("Cấu hình tốc độ rơi")]
    [Tooltip("Số càng cao gai rơi càng nhanh. Mặc định là 1, nên thử mức 3-5.")]
    public float tocDoRoi = 3f;

    private Rigidbody2D rb;
    private BoxCollider2D col;

    void Start()
    {
        // Lấy Component Rigidbody2D và BoxCollider2D
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();

        // Thiết lập các thông số để đối tượng rơi tự nhiên
        rb.bodyType = RigidbodyType2D.Dynamic;

        // Áp dụng số bạn chỉnh ở bên ngoài vào thuộc tính vật lý của Unity
        rb.gravityScale = tocDoRoi;

        // Đảm bảo đối tượng không bị bay lơ lửng
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.constraints = RigidbodyConstraints2D.None;

        // Tự động tích chọn 'Is Trigger' cho BoxCollider2D để gai đi xuyên qua mọi thứ
        col.isTrigger = true;
    }
}