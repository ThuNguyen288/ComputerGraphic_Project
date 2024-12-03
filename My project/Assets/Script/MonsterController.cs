using UnityEngine.UI;
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
    private bool isDeath = false;

    public Slider healthBar;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        UpdateHealthBar();

    }

    void Update()
    {

        if (isDeath) return;

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
        if (isDeath) return;

        // Transition to flying state
        animator.SetBool("isMoving", true);

        // Calculate direction vector towards player
        Vector2 direction = (player.position - transform.position).normalized;

        // Update monster's velocity to move towards player
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);

        // Flip the monster to face the player
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
        animator.SetBool("isMoving", false);

        // Kiểm tra va chạm với người chơi
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        foreach (Collider2D player in hitPlayers)
        {
            if (player.CompareTag("Player"))
            {
                // Gọi hàm TakeDamage của người chơi
                player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
                Debug.Log("Player hit by monster!");
            }
        }
    }


    private void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false); // Tắt Attack
        animator.SetBool("isMoving", true); // Quay lại Flying
    }


    public void TakeHit(int damage)
    {
        if (isDeath) return;

        currentHealth -= damage;
        UpdateHealthBar();
        animator.SetBool("isHit", true);
        animator.SetBool("isAttacking", false);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            Invoke(nameof(ResetHit), 0.1f);
        }
    }

    private void ResetHit()
    {
        animator.SetBool("isHit", false);
    }


    private void UpdateHealthBar()
    {
        if (healthBar != null)
            healthBar.value = (float)currentHealth / maxHealth;
    }


    private void Die()
    {
        isAttacking = false;
        isDeath = true;
        rb.linearVelocity = Vector2.zero; // Dừng mọi chuyển động
        animator.SetBool("isAttacking", false); // Chuyển sang Death
        animator.SetBool("isDeath", true); // Chuyển sang Death

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
