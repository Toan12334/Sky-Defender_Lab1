using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // public giúp bạn có thể chỉnh sửa tốc độ trực tiếp trên Unity Editor
    public float speed = 5f;

    // Update được gọi mỗi khung hình (frame)
    void Update()
    {
        // 1. Nhận tín hiệu từ bàn phím (A/D, Mũi tên Trái/Phải cho trục ngang; W/S, Mũi tên Lên/Xuống cho trục dọc)
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // 2. Tạo một Vector3 xác định hướng đi. (X: ngang, Y: cao/nhảy, Z: dọc)
        // Ở đây giả sử game 3D đi trên mặt phẳng nên Y = 0. Nếu game 2D, thay moveVertical vào trục Y.
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);

        // 3. Di chuyển nhân vật
        // Time.deltaTime giúp tốc độ di chuyển ổn định, không bị phụ thuộc vào độ mạnh yếu của máy tính
        transform.Translate(movement * speed * Time.deltaTime);
    }
}