using UnityEngine;

/*Author :Hungnd
 * Describe: Day la file xu ly damage cua bay gai va coc go 
 * Date:1/06/2026
*/

public class TrapDamage : MonoBehaviour
{
    [SerializeField] private GameManager manager;
    private AudioSource audioSource;

    private void Start()
    {
        // Lấy AudioSource gắn cùng đối tượng
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogWarning("Bạn chưa gắn AudioSource vào đối tượng: " + gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 1. Trừ máu
            if (manager != null)
            {
                manager.TakeDamage(10);
            }

            // 2. Phát âm thanh va chạm
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
                Debug.Log("Đã phát âm thanh bẫy!");
            }
        }
    }
}