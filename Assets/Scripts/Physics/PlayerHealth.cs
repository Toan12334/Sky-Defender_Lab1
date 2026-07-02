using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Có thể gán từ Inspector (cách tốt nhất)
    [SerializeField] private GameManager manager;
    [SerializeField] private SceneLoader loader;

    void Start()
    {
        // Hoặc tự động tìm trong Scene nếu quên gán
        if (manager == null) manager = FindAnyObjectByType<GameManager>();
        if (loader == null) loader = FindAnyObjectByType<SceneLoader>();
    }

    public void Die()
    {
        if (manager != null) manager.TakeDamage(100);
        if (loader != null) loader.LoadSceneByName("GameOver");
    }
}