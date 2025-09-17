using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [Header("Stats")]
    public int health = 100;
    public int maxHealth = 100;

    void Start()
    {
        // 游戏开始时，为玩家初始化一次UI
        if (this.CompareTag("Player"))
        {
            UIManager.Instance.UpdateHealth(health);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        // 确保血量不会低于0
        health = Mathf.Max(health, 0);

        if (this.CompareTag("Player"))
        {
            UIManager.Instance.UpdateHealth(health);
            if (health <= 0)
            {
                Die();
            }
        }
    }

    public void Heal(int healAmount)
    {
        // 1. 增加生命值
        health += healAmount;

        // 2. 使用 Mathf.Clamp 确保生命值不会超过上限
        health = Mathf.Clamp(health, 0, maxHealth);

        // 3. 更新UI
        if (this.CompareTag("Player"))
        {
            UIManager.Instance.UpdateHealth(health);
        }
    }

    private void Die()
    {
        if (this.CompareTag("Player"))
        {
            GameManager.Instance.GameOver();
            // 禁用玩家对象
            gameObject.SetActive(false);
        }
    }
}