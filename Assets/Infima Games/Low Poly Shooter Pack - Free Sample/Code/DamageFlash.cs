using UnityEngine;
using UnityEngine.UI;

public class DamageFlash : MonoBehaviour
{
    public Image damageImage;
    public PlayerHealth playerHealth;

    [Header("Hit Flash")]
    public float flashSpeed = 5f;
    public float hitAlpha = 0.5f;

    [Header("Low HP Effect")]
    public float lowHealthThreshold = 0.3f;
    public float maxLowHPAlpha = 0.4f;

    float targetAlpha = 0f;
    float timer;

    void Update()
    {
        float hpPercent = 1f;

        if (playerHealth != null)
        {
            hpPercent = (float)playerHealth.currentHealth / playerHealth.maxHealth;
        }

        float lowHPAlpha = 0f;

        // 🔥 Low HP → กะพริบ
        if (hpPercent <= lowHealthThreshold && playerHealth.currentHealth > 0)
        {
            timer += Time.deltaTime;

            float speed = Mathf.Lerp(1f, 6f, 1f - hpPercent);
            lowHPAlpha = Mathf.Abs(Mathf.Sin(timer * speed)) * maxLowHPAlpha;
        }

        // 🎯 เลือกค่า alpha ที่มากที่สุด (ไม่ให้ตีกัน)
        float finalAlpha = Mathf.Max(targetAlpha, lowHPAlpha);

        // smooth fade
        Color color = damageImage.color;
        color.a = Mathf.Lerp(color.a, finalAlpha, Time.deltaTime * flashSpeed);
        damageImage.color = color;

        // flash ค่อย ๆ หาย
        targetAlpha = Mathf.Lerp(targetAlpha, 0, Time.deltaTime * flashSpeed);
    }

    // 🔴 เรียกตอนโดนตี
    public void Flash()
    {
        targetAlpha = hitAlpha;
    }
}