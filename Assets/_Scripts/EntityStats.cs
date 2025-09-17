using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public int health;
    public int maxHealth = 100; // 新增最大生命值

    void Start()
    {
        // 游戏开始时初始化一次UI
        if (CompareTag("Player"))
        {
            UIManager.Instance.UpdateHealth(health);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        // 更新UI
        if (CompareTag("Player"))
        {
            UIManager.Instance.UpdateHealth(health);
        }
    }

    // 新增一个回血方法
    public void Heal(int healAmount)
    {
        health += healAmount;
        // 使用 Mathf.Clamp 确保血量不会超过上限
        health = Mathf.Clamp(health, 0, maxHealth);
        // 更新UI
        if (CompareTag("Player"))
        {
            UIManager.Instance.UpdateHealth(health);
        }
    }
}