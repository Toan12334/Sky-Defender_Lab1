
/*Author :Toandx
 * Describe: Day la file xu ly khi nhan vat cham nuoc thi se chet
 * Date:11/06/2026
*/
using UnityEngine;
public class WaterScripts:MonoBehaviour {


    [SerializeField] private GameManager manager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            manager.TakeDamage(100);
        }
    }

}
