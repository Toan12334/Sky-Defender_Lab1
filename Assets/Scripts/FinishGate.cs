using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishGate : MonoBehaviour
{
    // Tên của Cảnh chiến thắng mà Leader (Thành viên 1) đặt trong Build Settings
    [SerializeField] private string victorySceneName = "Victory";

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem đối tượng chạm vào cổng có Tag là Player hay không
        if (other.CompareTag("Player"))
        {
            // Tìm tất cả các viên ngọc còn lại trong màn chơi
            GemCollect[] remainingGems = FindObjectsOfType<GemCollect>();

            if (remainingGems.Length == 0)
            {
                Debug.Log("Chúc mừng! Bạn đã ăn hết ngọc và hoàn thành màn chơi!");
                SceneManager.LoadScene(victorySceneName);
            }
            else
            {
                Debug.Log($"Bạn chưa thể qua màn! Cần thu thập thêm {remainingGems.Length} viên ngọc nữa.");
            }
        }
    }
}
