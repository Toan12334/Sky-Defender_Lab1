using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [Header("UI Component")]
    public TextMeshProUGUI textComponent;
    public GameObject dialoguePanelObject; // Khung nền thoại để ẩn/hiện

    [Header("Cài đặt tốc độ")]
    [Tooltip("Thời gian delay giữa mỗi chữ (giây). Càng nhỏ chữ chạy càng nhanh.")]
    public float typingSpeed = 0.05f;

    [Header("Thời gian chờ ban đầu")]
    public float delayBeforeStart = 2.0f; // Số giây chờ trước khi hiện khung thoại

    private string fullText;
    private Coroutine typingCoroutine;

    void Start()
    {
        // Ban đầu ẩn toàn bộ khung thoại đi
        if (dialoguePanelObject != null)
        {
            dialoguePanelObject.SetActive(false);
        }

        // Gọi hàm kích hoạt sau một khoảng thời gian trì hoãn
        Invoke("TriggerFirstInstruction", delayBeforeStart);
    }

    void TriggerFirstInstruction()
    {
        // Hiện khung thoại lên
        if (dialoguePanelObject != null)
        {
            dialoguePanelObject.SetActive(true);
        }

        // Bắt đầu chạy chữ hướng dẫn
        ShowInstruction("Chào mừng Hiệp Sĩ! Hãy dùng các phím mũi tên để di chuyển.");
    }

    public void ShowInstruction(string message)
    {
        fullText = message;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        if (dialoguePanelObject != null && !dialoguePanelObject.activeSelf)
        {
            dialoguePanelObject.SetActive(true);
        }

        typingCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        textComponent.text = "";

        foreach (char letter in fullText.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    // HÀM MỚI: Hàm này sẽ được gọi khi bấm nút Tiếp tục để tắt Canvas thoại đi
    public void CloseDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); // Dừng chạy chữ nếu chưa chạy xong
        }

        if (dialoguePanelObject != null)
        {
            dialoguePanelObject.SetActive(false); // Ẩn khung thoại đi
        }
    }
}