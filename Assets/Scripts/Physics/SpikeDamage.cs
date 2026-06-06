using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    // Bạn có thể chỉnh lượng sát thương ngay trong bảng Inspector
    public int damageAmount = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Dòng này để test xem hai vật thể đã nhận diện Trigger chưa
        Debug.Log("Vật thể chạm vào gai là: " + collision.gameObject.name);

        if (collision.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TakeDamage(damageAmount);
                Debug.Log("Đã trừ " + damageAmount + " máu!");
            }
            else
            {
                Debug.LogWarning("GameManager.Instance đang bị null!");
            }
        }
    }
}