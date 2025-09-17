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
            // 【推荐优化】使用GetComponentInParent，代码更健壮
            EntityStats playerStats = other.GetComponentInParent<EntityStats>();

            // 检查是否真的找到了玩家的属性脚本
            if (playerStats == null) return;

            switch (type)
            {
                case CollectibleType.Coin:
                    GameManager.Instance.AddCoin(value);
                    break;
                case CollectibleType.HealthPotion:
                    // 【核心修正】调用Heal方法，而不是直接修改health变量
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