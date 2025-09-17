using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 真正地检查GameManager中玩家是否持有钥匙
            if (GameManager.Instance.hasKey)
            {
                GameManager.Instance.LevelComplete();
                // 可以在这里禁用玩家移动等
            }
            else
            {
                Debug.Log("The door is locked. Find the key!");
            }
        }
    }
}