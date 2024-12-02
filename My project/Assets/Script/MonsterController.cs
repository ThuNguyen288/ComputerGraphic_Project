using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    public float moveSpeed = 2f;
    public float detectionRange = 5f;
    public int maxHealth = 100;
    private int currentHealth;

    public Transform player;
    public Transform attackPoint;
    public float attackRange = 1f;
    public int attackDamage = 10;

    private bool isAttacking = false;
    private bool isHit = false;
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {

        if (isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && !isAttacking)
        {
            MoveTowardsPlayer();
        }

        if (distanceToPlayer <= attackRange && !isAttacking)
        {
            Attack();
        }

        if (distanceToPlayer >= attackRange && isAttacking)
        {
            EndAttack();
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        if (isDead) return;

        // Chuyển sang trạng thái bay
        animator.SetBool("isFlying", true);

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

        // Đổi hướng quái vật (quay mặt theo hướng người chơi)
        if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void Attack()
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero; // Dừng di chuyển

        animator.SetBool("isAttacking", true); // Bật trạng thái tấn công
        animator.SetBool("isFlying", false); // Bật trạng thái tấn công

        // Kiểm tra va chạm với người chơi
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        foreach (Collider2D player in hitPlayers)
        {
            if (player.CompareTag("Player"))
            {
                Debug.Log("Player hit!");
            }
        }

    }

    private void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false); // Tắt Attack
        animator.SetBool("isFlying", true); // Quay lại Flying
    }

    public void TakeHit(int damage)
    {
        if (isDead) return;

        isHit = true;
        currentHealth -= damage;

        animator.SetBool("isHit", true); // Kích hoạt animation bị đánh

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            Invoke(nameof(ResetHit), 0.5f);
        }
    }


    private void ResetHit()
    {
        isHit = false;
        animator.SetBool("isHit", false); // Tắt trạng thái Hit
        animator.SetBool("isFlying", true); // Quay lại trạng thái Flying
    }

    private void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero; // Dừng mọi chuyển động
        animator.SetBool("isDead", true); // Chuyển sang Death

        GetComponent<Collider2D>().enabled = false; // Vô hiệu hóa Collider
        Destroy(gameObject, 1f); // Xóa quái vật sau 1 giây
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        // Hiển thị bán kính tấn công trong Scene View
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    void LateUpdate()
    {
        // Đảm bảo quái vật luôn giữ nguyên Scale
        transform.localScale = new Vector3(5f, 5f, 1f);
    }

}
