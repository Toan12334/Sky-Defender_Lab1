using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Biến này để chứa nhân vật mà camera cần bám theo
    public Transform target;

    // Tốc độ camera di chuyển bám theo (số càng nhỏ bám càng chậm)
    public float smoothSpeed = 5f;

    // Khoảng cách từ camera đến nhân vật. 
    // Trục Z mặc định của Camera trong Unity 2D luôn là -10 để có thể nhìn thấy cảnh.
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    // Dùng LateUpdate thay vì Update cho Camera để tránh hiện tượng giật lag (jitter)
    // Camera sẽ di chuyển SAU KHI nhân vật đã di chuyển xong trong khung hình đó.
    void LateUpdate()
    {
        // Kiểm tra xem đã gắn nhân vật vào mục Target chưa
        if (target != null)
        {
            // Tính toán vị trí mà camera cần đi tới
            Vector3 desiredPosition = target.position + offset;

            // Dùng Vector3.Lerp để tạo hiệu ứng lướt đi mượt mà
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            // Gán vị trí mới cho Camera
            transform.position = smoothedPosition;
        }
    }
}