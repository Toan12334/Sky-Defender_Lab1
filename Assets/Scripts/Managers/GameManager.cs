using TMPro; // Sử dụng TextMeshPro
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Tạo Singleton để các class khác (như Quái vật, Đạn) dễ dàng gọi đến GameManager
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;

    [Header("Game Stats")]
    public int score = 0;
    public int health = 100;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateUI();
    }

    // Hàm tăng điểm (Khi bắn trúng địch...)
    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();

    }

    // Hàm trừ máu (Khi bị địch bắn trúng...)
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            SceneManager.LoadScene("GameOver"); // Thua game
        }
        UpdateUI();
    }

    // Cập nhật chữ hiển thị lên màn hình
    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (healthText != null) healthText.text = "HP: " + health + "%";
    }
}