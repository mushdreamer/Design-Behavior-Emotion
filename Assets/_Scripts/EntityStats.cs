using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [Header("Stats")]
    public int health = 100;
    public int maxHealth = 100;

    void Start()
    {
        // ��Ϸ��ʼʱ��Ϊ��ҳ�ʼ��һ��UI
        if (this.CompareTag("Player"))
        {
            UIManager.Instance.UpdateHealth(health);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        // ȷ��Ѫ���������0
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
        // --- ���������߼������� ---
        // ��ӡ���յ��������������ڵ���
        Debug.Log("Heal method called with amount: " + healAmount);

        // 1. ��������ֵ
        health += healAmount;

        // 2. ʹ�� Mathf.Clamp ȷ������ֵ���ᳬ������
        health = Mathf.Clamp(health, 0, maxHealth);

        // ��ӡ���ƺ������Ѫ�������ڵ���
        Debug.Log("Health after healing: " + health);

        // 3. ����UI
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
            // ������Ҷ���
            gameObject.SetActive(false);
        }
    }
}