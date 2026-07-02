

/*Author :Toandx
 * Describe: Đây là file quản lý chưa các máu ,điểm ,nhân vật trừ máu khi va chạm 
 * Date:11/06/2026
*/


using TMPro; // Sử dụng TextMeshPro
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Tạo Singleton để các class khác (nhu Quái vật, Đạn) dễ dàng gọi đến GameManager
    public static GameManager Instance { get; private set; }

    // ==========================================
    // KHU VỰC SỬA: Tạo 2 biến static ẩn để lưu dữ liệu xuyên màn chơi
    private static int savedScore = 0;
    private static int savedHealth = 100;
    // ==========================================

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;

    [Header("Game Stats")]
    public int score; // Bỏ gán = 0 ở đây để lấy từ bộ nhớ static
    public int health; // Bỏ gán = 100 ở đây để lấy từ bộ nhớ static

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // ==========================================
            // KHU VỰC SỬA: Nạp lại dữ liệu từ màn cũ vừa lưu sang màn mới
            score = savedScore;
            health = savedHealth;
            // ==========================================
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateUI();
    }

    // Hàm tăng điểm (Khi bắn trúng địch...)
    public void AddScore(int amount)
    {
        score += amount;
        savedScore = score; // KHU VỰC SỬA: Lưu lại vào bộ nhớ static ngay khi thay đổi
        UpdateUI();
    }

    private bool isGameOver = false;

    // Hàm trừ máu (Khi bị địch bắn trúng...)
    public void TakeDamage(int damage)
    {
        if (isGameOver) return; // Nếu đã chết rồi thì không trừ máu hay gọi lại hàm thua nữa

        health -= damage;
        if (health <= 0)
        {
            health = 0;
            isGameOver = true; // Đánh dấu đã chết

            // KHU VỰC SỬA: Thua game thì phải reset bộ nhớ về ban đầu để chơi lại không bị lỗi
            ResetStoredData();

            // Chờ 2 giây rồi mới chuyển sang màn hình GameOver để kịp xem animation Die
            Invoke("LoadGameOverScene", 2f);
            
            UpdateUI();
            return;
        }

        savedHealth = health; // KHU VỰC SỬA: Lưu lại vào bộ nhớ static ngay khi thay đổi
        UpdateUI();
    }

    private void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOver"); // Thua game
    }

    // Cập nhật chữ hiển thị lên màn hình
    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (healthText != null) healthText.text = "HP: " + health + "%";
    }

    // ==========================================
    // KHU VỰC THÊM MỚI: Hàm để reset dữ liệu về mặc định (Dùng khi Game Over hoặc bấm nút Back về MainMenu)
    public static void ResetStoredData()
    {
        savedScore = 0;
        savedHealth = 100;
    }
    // ==========================================
}