using UnityEngine;

public class StoneTrigger : MonoBehaviour
{
    [Header("Kéo cọc gỗ muốn rơi vào đây")]
    public FallingSpike targetSpike;

    // Hàm này tự chạy khi có ai đó dẫm lên hòn đá (va chạm vật lý)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra xem vật thể dẫm lên đá có mang Tag là "Player" không
        if (collision.gameObject.CompareTag("Player"))
        {
            if (targetSpike != null)
            {
                targetSpike.DropSpike(); // Ra lệnh cho cọc rơi xuống!
            }
        }
    }
}