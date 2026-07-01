using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [Header("Scene Loader Reference")]
    [Tooltip("Kéo thả GameObject chứa script SceneLoader vào đây")]
    public SceneLoader sceneLoader;

    [Header("Settings")]
    [Tooltip("Tên của Scene chiến thắng bạn muốn chuyển đến")]
    public string victorySceneName = "Victory";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Kiểm tra xem có đúng là Nhân vật (Player) bước vào không
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player đã bước vào cửa! Đang chuyển cảnh...");

            // 2. Kiểm tra xem đã kéo thả SceneLoader vào chưa
            if (sceneLoader != null)
            {
                // Gọi hàm LoadSceneByName từ script SceneLoader của bạn để chuyển đến màn Victory
                sceneLoader.LoadSceneByName(victorySceneName);

               
            }
            else
            {
              
                UnityEngine.SceneManagement.SceneManager.LoadScene(victorySceneName);
            }
        }
    }
}