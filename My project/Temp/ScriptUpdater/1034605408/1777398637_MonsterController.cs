using UnityEngine.UI;
using UnityEngine;
using System.Collections;

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

    public float attackCooldown = 1.0f; // Cooldown duration in seconds
    private bool canAttack = true; // Flag to track if the monster can attack

    public Slider healthBar;

    private Vector3 originalScale; // Store the original scale

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        // Save the original scale
        originalScale = transform.localScale;

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

        if (distanceToPlayer <= attackRange && !isAttacking && canAttack)
        {
            StartCoroutine(Attack());
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

        animator.SetBool("isMoving", true);

        Vector2 direction = (player.position - transform.position).normalized;

        rb.linearVelocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);

        // Adjust scale based on movement direction
        if (direction.x > 0)
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
        else if (direction.x < 0)
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        canAttack = false;
        rb.linearVelocity = Vector2.zero;

        animator.SetBool("isAttacking", true);
        animator.SetBool("isMoving", false);

        yield return new WaitForSeconds(0.5f); // Animation delay before dealing damage

        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        foreach (Collider2D player in hitPlayers)
        {
            if (player.CompareTag("Player"))
            {
                player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
                Debug.Log("Player hit by monster!");
            }
        }

        yield return new WaitForSeconds(attackCooldown); // Cooldown duration
        isAttacking = false;
        canAttack = true;
        EndAttack();
    }

    private void EndAttack()
    {
        animator.SetBool("isAttacking", false);
        animator.SetBool("isMoving", true);
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
        animator.SetBool("isAttacking", true);
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
        rb.linearVelocity = Vector2.zero;

        animator.SetBool("isAttacking", false);
        animator.SetBool("isDeath", true);

        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 1f);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
