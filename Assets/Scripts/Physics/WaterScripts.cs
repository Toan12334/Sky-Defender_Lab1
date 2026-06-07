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
