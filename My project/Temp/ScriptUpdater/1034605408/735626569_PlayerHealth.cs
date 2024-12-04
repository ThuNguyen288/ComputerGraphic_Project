using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Stats")]
    public int maxHealth = 200; // Máu tối đa của nhân vật
    private int currentHealth;  // Máu hiện tại

    [Header("UI Elements")]
    public Slider healthBar;    // Thanh máu trên UI

    [Header("Damage Settings")]
    public int baseDamage = 10; // Sát thương cơ bản
    private int currentDamage;  // Sát thương hiện tại (tăng khi dùng thuốc)
    public GameObject gameOverScreen; // Panel Game Over
    private bool isDead = false; // Track if the player is dead
    private Rigidbody2D rb; // To stop movement on death
    private GameStartController GameStartController;

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
        rb.linearVelocity = Vector2.zero;

        // Hiển thị màn hình Game Over
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true); // Show Game Over screen
        }

        // Stop any other gameplay actions (e.g., disable player controls)
        // Optionally disable player input or other necessary components here
        // For example, if you have a PlayerController script, you can disable it:
        // GetComponent<PlayerController>().enabled = false;

        // Show the Game Over screen through GameStartController
        if (GameStartController != null)
        {
            GameStartController.ShowGameOverScreen();
        }
    }
}



    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("HealingPotion"))
    //    {
    //        Heal(20); // Hồi 20 máu khi nhặt thuốc
    //        Destroy(collision.gameObject);
    //    }
    //    else if (collision.CompareTag("DamagePotion"))
    //    {
    //        IncreaseDamage(10); // Tăng 10 sát thương khi nhặt thuốc
    //        Destroy(collision.gameObject);
    //    }
    //
