using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // �����ؼ��GameManager������Ƿ����Կ��
            if (GameManager.Instance.hasKey)
            {
                GameManager.Instance.LevelComplete();
                // �����������������ƶ���
            }
            else
            {
                Debug.Log("The door is locked. Find the key!");
            }
        }
    }
}