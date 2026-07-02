/*Author :longnd
 * Describe: script cửa kiểm tra xem ăn hết gém chưa, sau khi ăn hết thì qua màn mới
 * Date:03/06/2026
*/
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScripts : MonoBehaviour
{
    // Tên scene chiến thắng cuối cùng khi người chơi thắng hết tất cả màn
    [SerializeField] private string victorySceneName = "Victory";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Kiểm tra còn gem không (validate ngay tại door)
            GemCollect[] remainingGems = FindObjectsOfType<GemCollect>();

            if (remainingGems.Length == 0)
            {
                // Đã ăn hết ngọc → chuyển cảnh
                int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

                if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
                {
                    Debug.Log("Chúc mừng! Đang chuyển sang màn tiếp theo...");
                    SceneManager.LoadScene(nextSceneIndex);
                }
                else
                {
                    Debug.Log("Chúc mừng! Bạn đã hoàn thành tất cả màn chơi!");
                    SceneManager.LoadScene(victorySceneName);
                }
            }
            else
            {
                Debug.Log($"Cửa chưa mở! Cần thu thập thêm {remainingGems.Length} viên ngọc nữa.");
            }
        }
    }
}
