using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadCurrentScene()
    {
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
