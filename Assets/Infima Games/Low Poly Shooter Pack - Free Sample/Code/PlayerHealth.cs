using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public DamageFlash damageFlash;

    [Header("Regen")]
    public int regenAmount = 1;        // ฟื้นครั้งละเท่าไหร่
    public float regenInterval = 2f;   // ทุกกี่วินาที

    float regenTimer;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        RegenHealth();
    }

    void RegenHealth()
    {
        // ถ้า HP เต็มแล้ว ไม่ต้องฟื้น
        if (currentHealth >= maxHealth)
            return;

        regenTimer += Time.deltaTime;

        if (regenTimer >= regenInterval)
        {
            currentHealth += regenAmount;

            // กัน HP เกิน
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;

            regenTimer = 0f;

            Debug.Log("Regen HP: " + currentHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        Debug.Log("HP: " + currentHealth);

        // 🔥 ทำให้จอแดง
        if (damageFlash != null)
        {
            damageFlash.Flash();
        }

        if (currentHealth <= 0)
        {
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.EndGame();
            }
        }
    }
}