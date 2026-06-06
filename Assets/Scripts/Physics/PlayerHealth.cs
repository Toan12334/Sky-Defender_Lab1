using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth = 3;

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player bị trừ máu! Máu còn: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Player đã chết!");
        // Thêm code xử lý khi chết ở đây (ví dụ: Load lại scene, hiện Panel GameOver, chạy anim chết...)
        // UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}