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
            // ���Ƽ��Ż���ʹ��GetComponentInParent���������׳
            EntityStats playerStats = other.GetComponentInParent<EntityStats>();

            // ����Ƿ�����ҵ�����ҵ����Խű�
            if (playerStats == null) return;

            switch (type)
            {
                case CollectibleType.Coin:
                    GameManager.Instance.AddCoin(value);
                    break;
                case CollectibleType.HealthPotion:
                    // ����������������Heal������������ֱ���޸�health����
                    playerStats.Heal(value);
                    break;
                case CollectibleType.Key:
                    GameManager.Instance.CollectKey();
                    break;
            }
            Destroy(gameObject);
        }
    }
}