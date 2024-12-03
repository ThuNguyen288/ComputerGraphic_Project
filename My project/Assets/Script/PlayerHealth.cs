using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Stats")]
    public int maxHealth = 100; // Máu tối đa của nhân vật
    private int currentHealth;  // Máu hiện tại

    [Header("UI Elements")]
    public Slider healthBar;    // Thanh máu trên UI

    [Header("Damage Settings")]
    public int baseDamage = 10; // Sát thương cơ bản
    private int currentDamage;  // Sát thương hiện tại (tăng khi dùng thuốc)

    void Start()
    {
        currentHealth = maxHealth;
        currentDamage = baseDamage;

        // Cập nhật thanh máu UI
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
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
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void IncreaseDamage(int amount)
    {
        currentDamage += amount;
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
            healthBar.value = (float)currentHealth / maxHealth;
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Thêm logic chết ở đây (VD: hiển thị màn hình Game Over)
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
    //}
}
