using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public int health;
    public int maxHealth = 100; // �����������ֵ

    void Start()
    {
        // ��Ϸ��ʼʱ��ʼ��һ��UI
        if (CompareTag("Player"))
        {
            UIManager.Instance.UpdateHealth(health);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        // ����UI
        if (CompareTag("Player"))
        {
            UIManager.Instance.UpdateHealth(health);
        }
    }

    // ����һ����Ѫ����
    public void Heal(int healAmount)
    {
        health += healAmount;
        // ʹ�� Mathf.Clamp ȷ��Ѫ�����ᳬ������
        health = Mathf.Clamp(health, 0, maxHealth);
        // ����UI
        if (CompareTag("Player"))
        {
            UIManager.Instance.UpdateHealth(health);
        }
    }
}