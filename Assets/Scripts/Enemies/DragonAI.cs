using UnityEngine;
// Gọi namespace hệ thống máu của bạn để code nhận diện được class Health
using ThomasDev.HealthDamageSystem;

public class DragonAI : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform poinA;
    public Transform poinB;
    public float patrolSpeed = 2f;

    [Header("Chase & Attack Settings")]
    public float chaseSpeed = 4f;
    public float detectionRadius = 5f;
    public float attackRadius = 2.5f;
    public LayerMask playerLayer;

    [Header("Attack Damage Settings")]
    public float damageAmount = 5f;    // Đổi thành float cho khớp với hàm TakeDamage(float amount) của bạn
    public float attackRate = 1.0f;
    private float nextAttackTime = 0f;

    [Header("UI Settings")]
    public Transform healthBarCanvas;

    private Transform targetPoint;
    private Transform player;
    private Animator anim;

    private const string ANIM_PATROL = "Move";
    private const string ANIM_CHASE = "Climb";
    private const string ANIM_ATTACK = "SpecialAttack";

    void Start()
    {
        anim = GetComponent<Animator>();
        if (poinA != null) targetPoint = poinA;
    }

    void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (hit != null)
        {
            player = hit.transform;
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRadius)
            {
                AttackPlayer();
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (poinA == null || poinB == null) return;

        anim.Play(ANIM_PATROL);
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, patrolSpeed * Time.deltaTime);
        FlipCharacter(targetPoint.position.x - transform.position.x);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            targetPoint = (targetPoint == poinA) ? poinB : poinA;
        }
    }

    void ChasePlayer()
    {
        anim.Play(ANIM_CHASE);
        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
        FlipCharacter(player.position.x - transform.position.x);
    }

    void AttackPlayer()
    {
        anim.Play(ANIM_ATTACK);

        // Rồng vừa khạc vừa bay đuổi theo Player
        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
        FlipCharacter(player.position.x - transform.position.x);

        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackRate;

            // Tìm chính xác component Health (thuộc hệ thống ThomasDev) trên Player
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                // Gọi hàm TakeDamage chuẩn chỉ từ file của bạn gửi
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }

    void FlipCharacter(float directionX)
    {
        Vector3 localScale = transform.localScale;

        if (directionX > 0.01f)
            localScale.x = -Mathf.Abs(localScale.x);
        else if (directionX < -0.01f)
            localScale.x = Mathf.Abs(localScale.x);

        transform.localScale = localScale;

        if (healthBarCanvas != null)
        {
            Vector3 hScale = healthBarCanvas.localScale;
            hScale.x = Mathf.Sign(transform.localScale.x) * Mathf.Abs(hScale.x);
            healthBarCanvas.localScale = hScale;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}