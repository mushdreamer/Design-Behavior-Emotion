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
                // �����ұȹ���ǿ
                if (playerStats.health > monsterStats.health)
                {
                    playerStats.TakeDamage(monsterStats.health);
                    Destroy(gameObject);
                    Debug.Log("Player defeated " + transform.name);
                }
                // �����ҵ��ڻ����ڹ���
                else
                {
                    Debug.Log("Player was defeated by " + transform.name);
                    // ��ʱ��ֻ��Ѫ����һ������ʵ������������
                    playerStats.TakeDamage(playerStats.health); // ���Ѫ������
                }
            }
        }
    }
}