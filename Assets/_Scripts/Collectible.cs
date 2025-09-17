using UnityEngine;

public class Collectible : MonoBehaviour
{
    // ����һ��������ö��������������Ʒ
    public enum CollectibleType
    {
        Coin,
        HealthPotion,
        Key
    }

    public CollectibleType type;
    public int value = 1; // ���������Ѫƿ�ظ���

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EntityStats playerStats = other.GetComponent<EntityStats>();

            switch (type)
            {
                case CollectibleType.Coin:
                    // ����GameManager��AddCoin����
                    GameManager.Instance.AddCoin(value);
                    break;
                case CollectibleType.HealthPotion:
                    if (playerStats != null)
                    {
                        playerStats.health += value;
                    }
                    break;
                case CollectibleType.Key:
                    // ����GameManager��CollectKey����
                    GameManager.Instance.CollectKey();
                    break;
            }
            Destroy(gameObject);
        }
    }
}