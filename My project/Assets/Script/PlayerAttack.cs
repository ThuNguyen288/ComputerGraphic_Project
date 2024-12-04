using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    public Transform attackPoint; // Điểm tấn công
    public float attackRange = 1f; // Phạm vi tấn công
    public int attackDamage = 20; // Sát thương
    public float attackCooldown = 0.5f; // Thời gian hồi chiêu

    private bool isAttacking = false;
    private float lastAttackTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time - lastAttackTime >= attackCooldown && !isAttacking)
        {
            Attack();
        }
    }

    private void Attack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        animator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy") || enemy.CompareTag("Boss"))
            {
                enemy.GetComponent<MonsterController>().TakeHit(attackDamage);
            }
        }

        Invoke(nameof(EndAttack), 0.3f);
    }

    private void EndAttack()
    {
        isAttacking = false;
    }


    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        // Hiển thị bán kính tấn công trong Scene View
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
