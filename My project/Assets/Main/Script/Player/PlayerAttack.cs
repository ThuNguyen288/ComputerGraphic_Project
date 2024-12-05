using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    [Header("Attack Settings")]
    public Transform attackPoint; // Điểm tấn công mặc định
    public float attackRange = 1f; // Phạm vi tấn công
    public int attackDamage = 20; // Sát thương
    public float attackCooldown = 0.5f; // Thời gian hồi chiêu

    private bool isAttacking = false;
    private float lastAttackTime;

    [Header("Weapon")]
    public Transform weaponHolder; // Vị trí trên tay nhân vật để gắn vũ khí
    private GameObject equippedWeapon; // Vũ khí đang được trang bị

    public bool HasWeapon
    {
        get { return equippedWeapon != null; } // True if a weapon is equipped, false otherwise
    }


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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            EquipWeapon(collision.gameObject); // Trang bị vũ khí khi va chạm với nó
        }
    }

    private void EquipWeapon(GameObject weapon)
    {
        if (equippedWeapon != null)
        {
            Destroy(equippedWeapon); // Remove previous weapon
        }

        equippedWeapon = Instantiate(weapon, weaponHolder.position, weaponHolder.rotation, weaponHolder);
        equippedWeapon.GetComponent<Collider2D>().enabled = false; // Disable weapon collider

        attackPoint = equippedWeapon.transform;

        Weapon weaponStats = equippedWeapon.GetComponent<Weapon>();
        if (weaponStats != null)
        {
            // Lấy giá trị baseDamage từ PlayerHealth
            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
            int baseDamage = playerHealth != null ? playerHealth.baseDamage : 10; // Default to 10 if not found
            attackDamage = baseDamage + weaponStats.damage; // Cập nhật sát thương tấn công
            attackRange = weaponStats.range; // Cập nhật phạm vi tấn công
        }

        // Update the player's damage in PlayerHealth
        PlayerHealth playerHealthUpdate = FindObjectOfType<PlayerHealth>();
        if (playerHealthUpdate != null)
        {
            playerHealthUpdate.UpdateDamageWithWeapon(); // Cập nhật sát thương trong PlayerHealth
        }

        // Destroy the weapon object from the ground
        Destroy(weapon);
    }




    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        // Hiển thị bán kính tấn công trong Scene View
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
