using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image healthBar; 
    public float changespeed = 5f;

    void Update()
    {
        if (Player.Instance == null || healthBar == null) return;

        float targetFill = Mathf.InverseLerp(0, Player.Instance.maxHealth, Player.Instance.currentHealth);
        
        float t = Mathf.Clamp01(changespeed * Time.deltaTime);
        float easedT = EaseOutCubic(t);
        
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, targetFill, easedT);
    }

    private float EaseOutCubic(float x)
    {
        return 1f - Mathf.Pow(1f - x, 3f);
    }
}