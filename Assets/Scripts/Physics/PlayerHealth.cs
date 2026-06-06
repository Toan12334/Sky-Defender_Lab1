using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private GameManager manager=new GameManager();
    private SceneLoader loader = new SceneLoader();
    public void Die()
    {
        manager.TakeDamage(100);
        loader.LoadSceneByName("GameOver");
        return;
    }
}