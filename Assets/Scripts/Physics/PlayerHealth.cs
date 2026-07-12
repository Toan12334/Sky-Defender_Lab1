using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private Animator animator;
    private bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        // =====================================================================
        // ĐOẠN ĐỒNG BỘ UI THÔNG MINH:
        // Tìm xem trên Player có script Health của asset không, nếu có thì ép nó 
        // kích hoạt sự kiện OnDamaged để làm thanh UI co lại ngay lập tức!
        // =====================================================================
        ThomasDev.HealthDamageSystem.Health assetHealth = GetComponent<ThomasDev.HealthDamageSystem.Health>();
        if (assetHealth != null)
        {
            // Ép script của asset cập nhật lại máu cho bằng với máu của mình
            // (Hàm này trong asset thường sẽ tự kích hoạt sự kiện OnDamaged để đổi fillAmount UI)
            assetHealth.TakeDamage((float)damage);
        }
        // =====================================================================

        if (animator != null)
        {
            animator.SetTrigger("takeDamage");
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        if (animator != null)
        {
            animator.SetTrigger("die");
        }

        GameManager.ResetStoredData();
        Invoke("LoadGameOverScene", 2f);
    }

    private void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }
}