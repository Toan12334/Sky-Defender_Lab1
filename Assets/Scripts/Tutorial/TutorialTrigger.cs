using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [Header("Cấu hình Lời thoại")]
    [TextArea(2, 5)]
    public string tutorialMessage = "Hãy nhấn nút SPACE để nhảy và vượt qua chướng ngại vật!";

    private TypewriterEffect typewriter;
    private bool hasTriggered = false; // Đảm bảo chỉ kích hoạt hướng dẫn 1 lần duy nhất

    void Start()
    {
        // Tự động tìm script chạy chữ đang nằm trên DialogueCanvas trong Game
        typewriter = Object.FindFirstObjectByType<TypewriterEffect>();
    }

    // Hàm này tự chạy khi Player đi vào vùng xanh Collider
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem có đúng là Player chạm vào không và đã từng kích hoạt chưa
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            if (typewriter != null)
            {
                typewriter.ShowInstruction(tutorialMessage); // Gọi chữ chạy lên
                hasTriggered = true; // Đánh dấu đã chạy xong hướng dẫn này
            }
        }
    }
}