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
            Debug.Log("Chúc mừng! Bạn đã đến đích và chiến thắng!");

            // Lệnh chuyển sang Scene chiến thắng
            SceneManager.LoadScene(victorySceneName);
        }
    }
}
