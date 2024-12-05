using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Stats")]
    public int maxHealth = 200; // Máu tối đa của nhân vật
    private int currentHealth;  // Máu hiện tại
    public int maxDamage = 200;
    [Header("UI Elements")]
    public Slider healthBar;    // Thanh máu trên UI

    [Header("Damage Settings")]
    public int baseDamage = 10; // Sát thương cơ bản
    private int currentDamage;  // Sát thương hiện tại (tăng khi dùng thuốc)
    public GameObject gameOverScreen; // Panel Game Over
    private bool isDead = false; // Track if the player is dead
    private Rigidbody2D rb; // To stop movement on death
    private GameStartController GameStartController;

    private PlayerAttack playerAttack;  // Khai báo tham chiếu đến PlayerAttack
    public Slider damageSlider;  // Tham chiếu đến Slider hiển thị sát thương

    void Start()
    {
        currentHealth = maxHealth;
        currentDamage = baseDamage;

        // Cập nhật thanh máu UI
        UpdateHealthBar();

        // Initialize GameOverScreen and GameStartController references
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);  // Ẩn Game Over panel khi bắt đầu game
        }

        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody2D to stop movement
        GameStartController = FindObjectOfType<GameStartController>(); // Get the GameStartController
        playerAttack = FindObjectOfType<PlayerAttack>(); // Tìm PlayerAttack trong scene
    }

    private void UpdateDamageBar()
    {
        if (damageSlider != null)
        {
            // Cập nhật giá trị của slider sao cho nó phản ánh sát thương hiện tại
            // Tính tỷ lệ sát thương hiện tại so với sát thương tối đa
            damageSlider.value = (float)currentDamage / (float)maxDamage;
        }

    }
        public void TakeDamage(int damage)
        {
            if (isDead) return; // Don't process damage if already dead

            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die(); // Gọi hàm chết nếu máu = 0
            }
            UpdateHealthBar();
        }

        public void Heal(int amount)
        {
            if (isDead) return; // Don't heal if already dead

            currentHealth += amount;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
            UpdateHealthBar();
        }

        public void IncreaseDamage(int amount)
        {
            if (isDead) return; // Don't increase damage if dead

            currentDamage += amount;
            if (playerAttack != null)
            {
                playerAttack.attackDamage = currentDamage; // Cập nhật attackDamage trong PlayerAttack
            }
        }

        private void UpdateHealthBar()
        {
            if (healthBar != null)
                healthBar.value = (float)currentHealth / maxHealth;
        }

        public void Die()
        {
            if (isDead) return; // Prevent multiple death executions

            // Đánh dấu Player đã chết
            isDead = true;

            // Dừng chuyển động của Player
            rb.linearVelocity = Vector2.zero; // Dừng chuyển động của nhân vật

            // Hiển thị màn hình Game Over
            if (gameOverScreen != null)
            {
                gameOverScreen.SetActive(true); // Show Game Over screen
            }

            // Show the Game Over screen through GameStartController
            if (GameStartController != null)
            {
                GameStartController.ShowGameOverScreen();
            }
        }

        public void ResetHealth()
        {
            currentHealth = maxHealth; // Đặt lại máu về tối đa
            UpdateHealthBar();         // Cập nhật lại thanh máu
        }

        public void UpdateDamageWithWeapon()
        {
            if (playerAttack != null && playerAttack.HasWeapon)
            {
                currentDamage = baseDamage + playerAttack.attackDamage; // Lấy sát thương từ PlayerAttack
            }
            else
            {
                currentDamage = baseDamage; // Mặc định sát thương là baseDamage nếu không có vũ khí
            }

            // Đảm bảo currentDamage không vượt quá giá trị maxDamage
            if (currentDamage > maxDamage)
            {
                currentDamage = maxDamage;
            }

            UpdateDamageBar(); // Cập nhật UI hoặc các thành phần UI liên quan đến sát thương
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("HealingPotion"))
            {
                Heal(20); // Hồi 20 máu khi nhặt thuốc
                Destroy(collision.gameObject);
            }
            else if (collision.CompareTag("DamagePotion"))
            {
                IncreaseDamage(10); // Tăng 10 sát thương khi nhặt thuốc
                Destroy(collision.gameObject);
            }
        }
    
}

