using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1; // Số máu bị trừ
    [SerializeField] private bool instantKill = false; // Tích chọn nếu muốn chạm vào chết luôn

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                if (instantKill)
                {
                    player.Die();
                }
                else
                {
                    player.TakeDamage(damageAmount);
                }
            }
        }
    }
}