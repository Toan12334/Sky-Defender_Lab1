using UnityEngine;

public class BoulderTrigger : MonoBehaviour
{
    // Dòng này BẮT BUỘC phải có [SerializeField] thì Unity mới hiện ô kéo thả ngoài Inspector
    [SerializeField] private RollingBoulder boulder;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Ếch đã giẫm trúng đất bẫy!"); // <-- Thêm dòng này để test
            if (boulder != null)
            {
                boulder.TriggerBoulder();
                Destroy(this);
            }
        }
    }
}