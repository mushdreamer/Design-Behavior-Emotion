using UnityEngine;

public class Collectible : MonoBehaviour
{
    // 定义一个公开的枚举类型来区分物品
    public enum CollectibleType
    {
        Coin,
        HealthPotion,
        Key
    }

    public CollectibleType type;
    public int value = 1; // 金币数量或血瓶回复量

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EntityStats playerStats = other.GetComponent<EntityStats>();

            switch (type)
            {
                case CollectibleType.Coin:
                    // 调用GameManager的AddCoin方法
                    GameManager.Instance.AddCoin(value);
                    break;
                case CollectibleType.HealthPotion:
                    if (playerStats != null)
                    {
                        playerStats.health += value;
                    }
                    break;
                case CollectibleType.Key:
                    // 调用GameManager的CollectKey方法
                    GameManager.Instance.CollectKey();
                    break;
            }
            Destroy(gameObject);
        }
    }
}