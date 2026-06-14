using UnityEngine;
/*Author :Hungnd
 * Describe: Day la file xu ly damage cua bay gai va coc go 
 * Date:1/06/2026
*/

public class TrapDamage : MonoBehaviour
{
    [SerializeField] private GameManager manager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            manager.TakeDamage(10);


        }
    }
}