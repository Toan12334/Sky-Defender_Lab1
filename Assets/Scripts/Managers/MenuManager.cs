

/*Author :Toandx
 * Describe: day la file chua cac ham cho nut click
 * Date:11/06/2026
*/

using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private SceneLoader sceneLoader;

    private void Start()
    {
        // Tự động tìm hoặc thêm SceneLoader vào cùng một Object để sử dụng
        sceneLoader = GetComponent<SceneLoader>();
        if (sceneLoader == null) sceneLoader = gameObject.AddComponent<SceneLoader>();
    }

    // Gán hàm này vào Event OnClick của nút Start
    public void OnClickStart()
    {
        sceneLoader.LoadSceneByName("Level4");
    }

    // Gán hàm này vào nút Restart
    public void OnClickRestart()
    {
        sceneLoader.LoadSceneByName("Level4");
    }

    // Gán hàm này vào nút MainMenu
    public void OnClickMainMenu()
    {
        sceneLoader.LoadSceneByName("MainMenu");
    }

    // Gán hàm này vào nút Quit
    public void OnClickQuit()
    {
        sceneLoader.QuitGame();
    }
}