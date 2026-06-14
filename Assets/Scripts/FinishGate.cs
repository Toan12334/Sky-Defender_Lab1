/*Author :longnd
 * Describe: Xử lý logic của cổng kiểm tra điều kiện hoàn thành màn chơi (FinishGate). 
 * Khi người chơi (Player) chạm vào, script sẽ kiểm tra xem đã thu thập hết ngọc (GemCollect) chưa. 
 * Nếu hết, kích hoạt trạng thái `isValidated = true` để cho phép qua màn thông qua DoorScripts.
 * Date:20/05/2026
*/
using UnityEngine;

public class FinishGate : MonoBehaviour
{
    // Cờ static để DoorScripts kiểm tra: đã validate (hết ngọc) chưa
    public static bool isValidated = false;

    private void Start()
    {
        // Reset lại mỗi khi vào màn mới
        isValidated = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem đối tượng chạm vào cổng có Tag là Player hay không
        if (other.CompareTag("Player"))
        {
            // Tìm tất cả các viên ngọc còn lại trong màn chơi
            GemCollect[] remainingGems = FindObjectsOfType<GemCollect>();

            if (remainingGems.Length == 0)
            {
                isValidated = true;
                Debug.Log("Đã ăn hết ngọc! Cổng đã được mở khóa. Hãy đến cửa (Door) để qua màn!");
            }
            else
            {
                Debug.Log($"Bạn chưa thể qua màn! Cần thu thập thêm {remainingGems.Length} viên ngọc nữa.");
            }
        }
    }
}
