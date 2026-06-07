using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneByName(string sceneName)
    {
        // ==========================================
        // KHU VỰC SỬA: Nếu chuyển về Menu chính, phải reset điểm và máu về mặc định
        // Bạn hãy đổi "MainMenu" thành tên Scene menu chính chính xác trong dự án của bạn nhé
        if (sceneName == "MainMenu" || sceneName == "Menu")
        {
            GameManager.ResetStoredData();
        }
        // ==========================================

        SceneManager.LoadScene(sceneName);
    }

    public void ReloadCurrentScene()
    {
        // ==========================================
        // KHU VỰC SỬA: Nếu người chơi bấm nút "Chơi lại" (Restart) khi đang chơi nửa chừng,
        // hoặc bạn dùng hàm này cho nút "Replay" ở màn hình GameOver, hãy reset dữ liệu:
        GameManager.ResetStoredData();
        // ==========================================

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Kiểm tra xem màn tiếp theo có tồn tại trong Build Settings không
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Đã hết màn chơi! Quay về màn hình chiến thắng hoặc menu chính.");
            SceneManager.LoadScene("Victory"); // Tên scene chiến thắng mặc định
        }
    }

    public void QuitGame()
    {
        Debug.Log("Game đã thoát!");
        Application.Quit();
    }
}