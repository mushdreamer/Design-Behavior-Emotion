using UnityEngine;

public class Monster : MonoBehaviour
{
    private EntityStats monsterStats;

    void Start()
    {
        monsterStats = GetComponent<EntityStats>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EntityStats playerStats = other.GetComponent<EntityStats>();
            if (playerStats != null)
            {
                // 如果玩家比怪物强
                if (playerStats.health > monsterStats.health)
                {
                    playerStats.TakeDamage(monsterStats.health);
                    Destroy(gameObject);
                    Debug.Log("Player defeated " + transform.name);
                }
                // 如果玩家等于或弱于怪物
                else
                {
                    Debug.Log("Player was defeated by " + transform.name);
                    // 暂时先只扣血，下一步我们实现真正的死亡
                    playerStats.TakeDamage(playerStats.health); // 玩家血量归零
                }
            }
        }
    }
}